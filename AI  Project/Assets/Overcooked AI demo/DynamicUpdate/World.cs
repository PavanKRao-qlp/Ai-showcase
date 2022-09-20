﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlantTest
{
    public class World : GOAP_World
    {       
        public World()
        {
            WorldStateSet = new GoapWorldState();
        }
        public void SetState(string key, object value)
        {
            WorldStateSet.UpdateValue(key, value);
        }
    }
}