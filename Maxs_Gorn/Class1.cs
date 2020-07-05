using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maxs_Gorn
{



        [Serializable]
        public class Music
        {
            public string Filename { get; set; }
            public string Category { get; set; }
        }
        [Serializable]
        public class ListMusic
        {
            public List<Music> Musics { get; set; } = new List<Music>();
        }
    

}
