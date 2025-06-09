using System;
using System.Runtime.InteropServices;
using K4os.Compression.LZ4.Engine;

namespace K4os.Compression.LZ4
{
	/// <summary>
	/// Static class exposing LZ4 block compression methods.
	/// </summary>
	public class LZ4Codec
	{
		/// <summary>Maximum size after compression.</summary>
		/// <param name="length">Length of input buffer.</param>
		/// <returns>Maximum length after compression.</returns>
		public static int MaximumOutputSize(int length) =>
			LZ4_xx.LZ4_compressBound(length);

		/// <summary>Compresses data from one buffer into another.</summary>
		/// <param name="source">Input buffer.</param>
		/// <param name="sourceLength">Length of input buffer.</param>
		/// <param name="target">Output buffer.</param>
		/// <param name="targetLength">Output buffer length.</param>
		/// <param name="level">Compression level.</param>
		/// <returns>Number of bytes written, or negative value if output buffer is too small.</returns>
		public static unsafe int Encode(
			byte* source, int sourceLength,
			byte* target, int targetLength,
			LZ4Level level = LZ4Level.L00_FAST)
		{
			if (sourceLength <= 0)
				return 0;

			var encoded =
				level == LZ4Level.L00_FAST
					? LZ4_64.LZ4_compress_default(source, target, sourceLength, targetLength)
					: LZ4_64_HC.LZ4_compress_HC(
						source, target, sourceLength, targetLength, (int) level);
			return encoded <= 0 ? -1 : encoded;
		}

#if NETSTANDARD2_1
		/// <summary>Compresses data from one buffer into another.</summary>
		/// <param name="source">Input buffer.</param>
		/// <param name="target">Output buffer.</param>
		/// <param name="level">Compression level.</param>
		/// <returns>Number of bytes written, or negative value if output buffer is too small.</returns>
		public static unsafe int Encode(
			ReadOnlySpan<byte> source, Span<byte> target,
			LZ4Level level = LZ4Level.L00_FAST)
		{
			var sourceLength = source.Length;
			if (sourceLength <= 0)
				return 0;

			var targetLength = target.Length;
			fixed (byte* sourceP = &MemoryMarshal.GetReference(source))
			fixed (byte* targetP = &MemoryMarshal.GetReference(target))
				return Encode(sourceP, sourceLength, targetP, targetLength, level);
		}
#endif

		/// <summary>Compresses data from one buffer into another.</summary>
		/// <param name="source">Input buffer.</param>
		/// <param name="sourceOffset">Input buffer offset.</param>
		/// <param name="sourceLength">Input buffer length.</param>
		/// <param name="target">Output buffer.</param>
		/// <param name="targetOffset">Output buffer offset.</param>
		/// <param name="targetLength">Output buffer length.</param>
		/// <param name="level">Compression level.</param>
		/// <returns>Number of bytes written, or negative value if output buffer is too small.</returns>
		public static unsafe int Encode(
			byte[] source, int sourceOffset, int sourceLength,
			byte[] target, int targetOffset, int targetLength,
			LZ4Level level = LZ4Level.L00_FAST)
		{
			source.Validate(sourceOffset, sourceLength);
			target.Validate(targetOffset, targetLength);
			fixed (byte* sourceP = source)
			fixed (byte* targetP = target)
				return Encode(
					sourceP + sourceOffset, sourceLength,
					targetP + targetOffset, targetLength,
					level);
		}

		/// <summary>Decompresses data from given buffer.</summary>
		/// <param name="source">Input buffer.</param>
		/// <param name="sourceLength">Input buffer length.</param>
		/// <param name="target">Output buffer.</param>
		/// <param name="targetLength">Output buffer length.</param>
		/// <returns>Number of bytes written, or negative value if output buffer is too small.</returns>
		public static unsafe int Decode(
			byte* source, int sourceLength,
			byte* target, int targetLength)
		{
			if (sourceLength <= 0)
				return 0;

			var decoded = LZ4_xx.LZ4_decompress_safe(source, target, sourceLength, targetLength);
			return decoded <= 0 ? -1 : decoded;
		}

#if NETSTANDARD2_1
		/// <summary>Decompresses data from given buffer.</summary>
		/// <param name="source">Input buffer.</param>
		/// <param name="target">Output buffer.</param>
		/// <returns>Number of bytes written, or negative value if output buffer is too small.</returns>
		public static unsafe int Decode(
			ReadOnlySpan<byte> source, Span<byte> target)
		{
			var sourceLength = source.Length;
			if (sourceLength <= 0)
				return 0;

			var targetLength = target.Length;
			fixed (byte* sourceP = &MemoryMarshal.GetReference(source))
			fixed (byte* targetP = &MemoryMarshal.GetReference(target))
				return Decode(sourceP, sourceLength, targetP, targetLength);
		}
#endif
		
		/// <summary>Decompresses data from given buffer.</summary>
		/// <param name="source">Input buffer.</param>
		/// <param name="sourceOffset">Input buffer offset.</param>
		/// <param name="sourceLength">Input buffer length.</param>
		/// <param name="target">Output buffer.</param>
		/// <param name="targetOffset">Output buffer offset.</param>
		/// <param name="targetLength">Output buffer length.</param>
		/// <returns>Number of bytes written, or negative value if output buffer is too small.</returns>
		public static unsafe int Decode(
			byte[] source, int sourceOffset, int sourceLength,
			byte[] target, int targetOffset, int targetLength)
		{
			source.Validate(sourceOffset, sourceLength);
			target.Validate(targetOffset, targetLength);
			fixed (byte* sourceP = source)
			fixed (byte* targetP = target)
				return Decode(
					sourceP + sourceOffset, sourceLength,
					targetP + targetOffset, targetLength);
		}
	}
}
