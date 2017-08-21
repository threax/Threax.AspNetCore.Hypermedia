using System;
using System.IO;
using System.Net.Http;
using Xunit;

namespace Threax.AspNetCore.Halcyon.Client.Tests
{
    /// <summary>
    /// These tests don't really verify anything
    /// </summary>
    public class FormBuilderTests
    {
        class SimpleQueryTest
        {
            public String Name { get; set; }

            public int Number { get; set; }
        }

        [Fact]
        public void BasicForm()
        {
            using (var form = new MultipartFormDataContent())
            {
                FormContentBuilder.BuildFormContent(new SimpleQueryTest()
                {
                    Name = "Bob",
                    Number = 1
                }, form);
            }
        }

        [Fact]
        public void EncodedForm()
        {
            using (var form = new MultipartFormDataContent())
            {
                FormContentBuilder.BuildFormContent(new SimpleQueryTest()
                {
                    Name = "Bob Smith & The Crew / Other Peeps",
                    Number = 1
                }, form);
            }
        }

        class ArrayQueryTest
        {
            public String Name { get; set; }

            public int[] Numbers { get; set; }
        }

        [Fact]
        public void ArrayForm()
        {
            using (var form = new MultipartFormDataContent())
            {
                FormContentBuilder.BuildFormContent(new ArrayQueryTest()
                {
                    Name = "Bob",
                    Numbers = new int[] { 1, 15, 20 }
                }, form);
            }
        }

        class FileTest
        {
            public String Name { get; set; }

            public Stream Stream { get; set; }
        }

        [Fact]
        public void StreamForm()
        {
            using (var form = new MultipartFormDataContent())
            {
                using(var streamWriter = new StreamWriter(new MemoryStream()))
                {
                    streamWriter.WriteLine("This is a test of a stream");
                    FormContentBuilder.BuildFormContent(new FileTest()
                    {
                        Name = "Bob",
                        Stream = streamWriter.BaseStream
                    }, form);
                }
            }
        }
    }
}
