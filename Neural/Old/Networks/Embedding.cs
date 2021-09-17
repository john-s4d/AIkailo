//////////////////////////////////////////////////////////////////////////////////////////
// ANNdotNET - Deep Learning Tool                                                       //
// Copyright 2017-2018 Bahrudin Hrnjica                                                 //
//                                                                                      //
// This code is free software under the MIT License                                     //
// See license section of  https://github.com/bhrnjica/anndotnet/blob/master/LICENSE.md  //
//                                                                                      //
// Bahrudin Hrnjica                                                                     //
// bhrnjica@hotmail.com                                                                 //
// Bihac, Bosnia and Herzegovina                                                         //
// http://bhrnjica.net                                                                  //
//////////////////////////////////////////////////////////////////////////////////////////
using AIkailo.Neural.Core;
using CNTK;

namespace AIkailo.Neural.Builder
{
    /// <summary>
    /// Implementation of Embedding layer. 
    /// </summary>
    public class Embedding : Network
    {
        /// <summary>
        /// Create embedding layer
        /// </summary>
        /// <param name="input"></param>
        /// <param name="embeddingDim"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        public static Function Create(Variable x, int embeddingDim, DataType dataType, DeviceDescriptor device, uint seed, string name)
        {
            var f = new Network();
            //weights
            var W = f.Weights(embeddingDim, dataType, device, seed, "e");

            //matrix product
            var Wx = CNTKLib.Times(W, x, name);

            //
            return Wx;
        }
    }
}
