using com.IronicEntertainment.Scripts.Data;
using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

// Author: Ironee

namespace com.IronicEntertainment.Scripts.Tools
{
    public static class IR_Extensions
    {
        public static T DuplicateData<T>(this T pResource, T target) where T : Ironee_Resource
        {
            PropertyInfo[] lProperties = pResource.GetType().GetProperties();

            foreach (PropertyInfo pi in lProperties)
            {
                if (pi.Name == nameof(pResource.Name) || !pi.CanWrite) continue;

                object value = pi.GetValue(target);
                pi.SetValue(pResource, value);
            }

            pResource.Export();
            return pResource;
        }
    }
}