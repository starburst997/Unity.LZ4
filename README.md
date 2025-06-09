# Unity.LZ4

LZ4 is lossless compression algorithm, sacrificing compression ratio for compression/decompression speed. Its compression speed is ~400 MB/s per core while decompression speed reaches ~2 GB/s, not far from RAM speed limits.

This is my personal fork for use in Unity project.

## Installation

Add the dependency to your `manifest.json`

```json
{
  "dependencies": {
    "jd.boiv.in.lz4": "https://github.com/starburst997/Unity.LZ4.git"
  }
}
```

## Usage

Simply call the following to compress an array of bytes:

```csharp
// `source` is an array of bytes
var target = new byte[LZ4Codec.MaximumOutputSize(source.Length)];
var encodedLength = LZ4Codec.Encode(
    source, 0, source.Length,
    target, 0, target.Length, LZ4Level.L00_FAST);

// `target` has the compressed bytes
writer.Write(target, 0, encodedLength);
```

## TODO

- Better readme

## Credits

Based on [K4os.Compression.LZ4](https://github.com/MiloszKrajewski/K4os.Compression.LZ4) which is based on the og [LZ4](https://github.com/lz4/lz4).