﻿using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using PCLStorage;

namespace VoiceRecognition.Services
{
    class MediaRequestHelper
    {
        const int BufferSize = 1024;

        public static HttpContent PopulateRequestContent(string audioFilePath)
        {
            return new PushStreamContent(async (outputStream, httpContext, transportContext) =>
            {
                try
                {
                    byte[] buffer = null;
                    int bytesRead = 0;

                    var file = await FileSystem.Current.GetFileFromPathAsync(audioFilePath);

                    using (outputStream) //must close/dispose output stream to notify that content is done
                    using (var audioStream = await file.OpenAsync(FileAccess.Read))
                    {
                        //read 1024 (BufferSize) (max) raw bytes from the input audio file
                        buffer = new Byte[checked((uint)Math.Min(BufferSize, (int)audioStream.Length))];

                        while ((bytesRead = await audioStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                        {
                            await outputStream.WriteAsync(buffer, 0, bytesRead);
                        }

                        await outputStream.FlushAsync();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
            }, new MediaTypeHeaderValue(Config.MimeTypes.WavAudio));
        }

        //public static float CheckAudioLength(string audioFilePath, long bitsPerFrame, int channels, long framesPerSeconds)
        //{
        //    long bytesPerSecond = (bitsPerFrame / 8) * channels * framesPerSeconds;
        //    float Secondslength = 0.0f;
        //    try
        //    {
        //        var fileTask =  FileSystem.Current.GetFileFromPathAsync(audioFilePath);
        //        fileTask.Wait();
        //        using (var audioStream = await file.OpenAsync(FileAccess.Read))
        //        {
        //            long ByteLength = audioStream.Length;
        //            Secondslength = ByteLength / bytesPerSecond;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //        throw;
        //    }

        //    return Secondslength;
        //}
    }
}