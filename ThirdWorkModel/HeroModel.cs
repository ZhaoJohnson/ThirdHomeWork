﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdWorkInterFace;

namespace ThirdWorkModel
{
    public class HeroModel: IHeroModel
    {
        public string MyHero { get; set; }
        public string[] HeroPosition { get; set; }
    }
}