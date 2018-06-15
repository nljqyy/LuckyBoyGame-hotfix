using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public sealed class Version
{
    public int One { get; private set; }
    public int Two { get; private set; }
    public int Three { get; private set; }
    public string Content { get; private set; }
    public ulong ContentLength { get; private set; }

    public Version(string _version)
    {
        string[] vers = _version.Split('.');
        if (vers.Length > 1)
        {
            One = Convert.ToInt32(vers[0]);
            Two = Convert.ToInt32(vers[1]);
            Three = Convert.ToInt32(vers[2]);
        }
        vers = _version.Split('-');
        Content = vers[0];
        if (vers.Length > 1)
            ContentLength = Convert.ToUInt64(vers[1]);
    }



    public bool CompareVersion(Version v)
    {
        if (One < v.One)
            return true;
        if (Two < v.Two)
            return true;
        if (Three < v.Three)
            return true;
        return false;
    }
}
