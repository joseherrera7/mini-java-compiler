using System;
using System.IO;
using System.Reflection;
using System.Resources;

namespace mini_java_compiler.Utilidades
{
    public sealed class ResourceUtil
    {
        private ResourceUtil()
        { }

        public static void AddFileAsStringResource(string resourceFile,
                                                   string resourceName,
                                                   string inputFile)
        {
            ResourceWriter writer = new ResourceWriter(resourceFile);
            StreamReader reader = new StreamReader(inputFile);
            string s = reader.ReadToEnd();
            reader.Close();
            writer.AddResource(resourceName, s);
            writer.Close();
        }

        public static string GetStringResource(Assembly assembly,
                                               string baseName,
                                               string resourceName)
        {
            System.Resources.ResourceManager rm =
                new System.Resources.ResourceManager(baseName, assembly);
            return (String)rm.GetObject(resourceName);
        }

        public static byte[] GetByteArrayResource(Assembly assembly,
                                                  string baseName,
                                                  string resourceName)
        {
            System.Resources.ResourceManager rm =
                new System.Resources.ResourceManager(baseName, assembly);
            return (System.Byte[])rm.GetObject(resourceName);
        }

    }
}
