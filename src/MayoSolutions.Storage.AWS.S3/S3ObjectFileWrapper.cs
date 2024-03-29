﻿using Amazon.S3.Model;

namespace MayoSolutions.Storage.AWS.S3
{
    internal class S3ObjectFileWrapper : IFile
    {
        public string Name { get; protected set; }
        public string Path { get; protected set; }
        public long? Size { get; protected set; }

        public S3ObjectFileWrapper(
            string path,
            S3Object fileObject
            )
        {
            Path = path;
            Name = fileObject.Key;
            Size = fileObject.Size;
        }
        public S3ObjectFileWrapper(
            string path,
            GetObjectResponse response
            )
        {
            Path = path;
            Name = response.Key;
            Size = null; // TODO: Size from a response
        }
    }
}