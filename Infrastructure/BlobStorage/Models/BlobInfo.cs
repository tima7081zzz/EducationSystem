﻿namespace BlobStorage.Models;

public class BlobInfo
{
    public required string BlobName { get; set; }
    public required Uri Uri { get; set; }
}