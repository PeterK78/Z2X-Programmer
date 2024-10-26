using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z2XProgrammer.DataModel
{
    public class LocoListType
    {

        /// <summary>
        /// User defined decoder description.
        /// </summary>
        public string UserDefindedDecoderDescription { get; set; } = string.Empty;

        /// <summary>
        /// Locomotive address.
        /// </summary>
        public ushort LocomotiveAddress { get; set;  }

        /// <summary>
        /// Path to the Z2X file.
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Stores the use defined image.
        /// </summary>
        public ImageSource UserDefindedImage { get; set; } = string.Empty;

        public LocoListType() { }

    }
}
