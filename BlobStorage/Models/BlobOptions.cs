﻿namespace BlobStorage;

public class BlobOptions
{
    public const string SectionName = "Blob";
    public required string BlobConnectionString { get; set; }
}