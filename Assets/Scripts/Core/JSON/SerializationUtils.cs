using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using System.Text;

public interface JSONSerializable
{
    Hashtable ToJSON();
}

public class SerializationUtils
{
    public static string EncodeMeshToBase64OBJ(Mesh mesh)
    {
        StringBuilder str = new StringBuilder();
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector2[] uvs = mesh.uv;

        int[] triangles = mesh.triangles;

        for (int i = 0; i < vertices.Length; i++ )
        {
            Vector3 v = vertices[i];
            str.AppendLine(String.Format("v {0} {1} {2}", v.x, v.y, v.z));
        }

        for (int i = 0; i < normals.Length; i++)
        {
            Vector3 vn = normals[i];
            str.AppendLine(String.Format("vn {0} {1} {2}", vn.x, vn.y, vn.z));
        }

        for (int i = 0; i < uvs.Length; i++)
        {
            Vector2 vt = uvs[i];
            str.AppendLine(String.Format("vt {0} {1}", vt.x, vt.y));
        }

        for (int i = 0; i < triangles.Length / 3; i++)
        {
            int v1 = triangles[i * 3] + 1;
            int v2 = triangles[i * 3 + 1] + 1;
            int v3 = triangles[i * 3 + 2] + 1;

            str.AppendLine(String.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}", v1, v2, v3));
        }

        //System.IO.File.WriteAllText(Application.dataPath + "/test.obj", str.ToString());

        byte[] bytes = Encoding.UTF8.GetBytes(str.ToString());
        return Convert.ToBase64String(bytes);
    }

    public static string EncodePNGToBase64(Texture2D texture)
    {
        return Convert.ToBase64String(texture.EncodeToPNG());
    }

    public static Hashtable ToJSON(object obj)
    {
        if (typeof(JSONSerializable).IsAssignableFrom(obj.GetType()))
            return ((JSONSerializable)obj).ToJSON();

        return BuildHashtableFromObject(obj);
    }
    
    public static ArrayList ToJSON(object[] array)
    {
        ArrayList htArray = new ArrayList(array.Length);

        for (int i = 0; i < array.Length; i++)
            htArray.Add(ToJSON(array[i]));

        return htArray;
    }

    protected static Hashtable BuildHashtableFromObject(object obj)
    {
        Hashtable ht = new Hashtable();
        BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        foreach (FieldInfo field in obj.GetType().GetFields(bindingFlags))
        {
            if (field.FieldType.IsPrimitive)
                ht[field.Name] = field.GetValue(obj);
            else
                ht[field.Name] = ToJSON(field.GetValue(obj));
        }

        return ht;
    }
    
    private const string INDENT_STRING = "    ";

    // Taken from http://stackoverflow.com/questions/4580397/json-formatter-in-c
    // I modified it to remove an additional space when appending a line
    public static string FormatJson(string str)
    {
        var indent = 0;
        var quoted = false;
        var sb = new StringBuilder();
        var lastChar = ' ';

        for (var i = 0; i < str.Length; i++)
        {
            var ch = str[i];
            switch (ch)
            {
                case '{':
                case '[':
                    sb.Append(ch);
                    if (!quoted)
                    {
                        sb.AppendLine();
                        Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
                    }
                    break;
                case '}':
                case ']':
                    if (!quoted)
                    {
                        sb.AppendLine();
                        Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
                    }
                    sb.Append(ch);
                    break;
                case '"':
                    sb.Append(ch);
                    bool escaped = false;
                    var index = i;
                    while (index > 0 && str[--index] == '\\')
                        escaped = !escaped;
                    if (!escaped)
                        quoted = !quoted;
                    break;
                case ',':
                    sb.Append(ch);
                    if (!quoted)
                    {
                        sb.AppendLine();
                        Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
                    }
                    break;
                case ':':
                    sb.Append(ch);
                    if (!quoted)
                        sb.Append(" ");
                    break;
                case ' ':
                    if(quoted || lastChar != ',')
                        sb.Append(ch);
                    break;
                default:
                    sb.Append(ch);
                    break;
            }

            lastChar = ch;
        }
        return sb.ToString();
    }

}

public static class Extensions
{
    public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
    {
        foreach (var i in ie)
            action(i);
    }
}