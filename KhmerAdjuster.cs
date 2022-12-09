using System.Globalization;
using System.Text;
using UnityEngine;

public class KhmerAdjuster
{
    /// <summary>
    /// 用于显示一段文本的unicode编码
    /// </summary>
    /// <param name="source">需要查看unicode的文本</param>
    /// <returns></returns>
    public static string StringToUnicode(string source)
    {
        byte[] bytes = Encoding.Unicode.GetBytes(source);
        StringBuilder sb = new();
        for (int i = 0; i < bytes.Length; i += 2)
        {
            sb.AppendFormat("{0:X2}{1:X2} ", bytes[i + 1], bytes[i]);
        }

        return sb.ToString();
    }

    /// <summary>
    /// 处理52个重叠辅音
    /// </summary>
    /// <param name="source">柬埔寨语原文本</param>
    /// <returns></returns>
    private static string Mapping52DoubleConsonants(string source)
    {
        // 1780
        source = source.Replace("\u17D2\u1780", "\u1000");
        source = source.Replace("\u17D2\u1781", "\u1001");
        source = source.Replace("\u17D2\u1782", "\u1002");
        source = source.Replace("\u17D2\u1783", "\u1003");
        source = source.Replace("\u17D2\u1784", "\u1004");
        source = source.Replace("\u17D2\u1785", "\u1005");
        source = source.Replace("\u17D2\u1786", "\u1006");
        source = source.Replace("\u17D2\u1787", "\u1007");
        source = source.Replace("\u17D2\u1788", "\u1008");
        source = source.Replace("\u17D2\u1789", "\u1009");
        source = source.Replace("\u17D2\u178A", "\u1010");
        source = source.Replace("\u17D2\u178B", "\u1011");
        source = source.Replace("\u17D2\u178C", "\u1012");
        source = source.Replace("\u17D2\u178D", "\u1013");
        source = source.Replace("\u17D2\u178E", "\u1014");
        source = source.Replace("\u17D2\u178F", "\u1015");
        // 1790
        source = source.Replace("\u17D2\u1790", "\u1016");
        source = source.Replace("\u17D2\u1791", "\u1017");
        source = source.Replace("\u17D2\u1792", "\u1018");
        source = source.Replace("\u17D2\u1793", "\u1019");
        source = source.Replace("\u17D2\u1794", "\u1020");
        source = source.Replace("\u17D2\u1795", "\u1021");
        source = source.Replace("\u17D2\u1796", "\u1022");
        source = source.Replace("\u17D2\u1797", "\u1023");
        source = source.Replace("\u17D2\u1798", "\u1024");
        source = source.Replace("\u17D2\u1799", "\u1025");
        source = source.Replace("\u17D2\u179A", "\u1026");
        source = source.Replace("\u17D2\u179B", "\u1027");
        source = source.Replace("\u17D2\u179C", "\u1028");
        source = source.Replace("\u17D2\u179D", "\u1029");
        source = source.Replace("\u17D2\u179E", "\u1030");
        source = source.Replace("\u17D2\u179F", "\u1031");
        // 17A0
        source = source.Replace("\u17D2\u17A0", "\u1032");
        source = source.Replace("\u17D2\u17A1", "\u1033");
        source = source.Replace("\u17D2\u17A2", "\u1034");
        source = source.Replace("\u17D2\u17A3", "\u1035");
        source = source.Replace("\u17D2\u17A4", "\u1036");
        source = source.Replace("\u17D2\u17A5", "\u1037");
        source = source.Replace("\u17D2\u17A6", "\u1038");
        source = source.Replace("\u17D2\u17A7", "\u1039");
        source = source.Replace("\u17D2\u17A8", "\u1040");
        source = source.Replace("\u17D2\u17A9", "\u1041");
        source = source.Replace("\u17D2\u17AA", "\u1042");
        source = source.Replace("\u17D2\u17AB", "\u1043");
        source = source.Replace("\u17D2\u17AC", "\u1044");
        source = source.Replace("\u17D2\u17AD", "\u1045");
        source = source.Replace("\u17D2\u17AE", "\u1046");
        source = source.Replace("\u17D2\u17AF", "\u1047");
        // 17B0
        source = source.Replace("\u17D2\u17B0", "\u1048");
        source = source.Replace("\u17D2\u17B1", "\u1049");
        source = source.Replace("\u17D2\u17B2", "\u1050");
        source = source.Replace("\u17D2\u17B3", "\u1051");

        return source;
    }

    /// <summary>
    /// 处理5个包围型元音
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private static string Mapping5RoundVowel(string source)
    {
        source = source.Replace("\u17BE", "\u1060\u1060");
        source = source.Replace("\u17BF", "\u1061\u1061");
        source = source.Replace("\u17C0", "\u1062\u1062");
        source = source.Replace("\u17C4", "\u1063\u1063");
        source = source.Replace("\u17C5", "\u1064\u1064");

        return source;
    }

    /// <summary>
    /// 移动给定unicode到组合的最左边，使用于移动包围型元音左边部分和显示在左边的元音
    /// </summary>
    /// <param name="array">unicode字符串数组</param>
    /// <param name="index">要移动字符在数组中的索引</param>
    private static void MoveVowelToLeft(string[] array, int index)
    {
        string code = array[index];
        int pivot = index - 1;
        while (pivot >= 0)
        {
            int codeNumber = int.Parse(array[pivot], NumberStyles.HexNumber);
            // code是单字型辅音
            if (codeNumber >= 0x1780 && codeNumber <= 0x17A2)
            {
                // 处理左边是1026重叠辅音的情况，往左再移一位
                // 因为按u split后，第一位是空字符串，所以要求pivot>1
                if (pivot > 1 && int.Parse(array[pivot - 1], NumberStyles.HexNumber) == 0x1026)
                {
                    pivot--;
                }

                // 把pivot开始的字往后移
                int pivotMove = index;
                while (pivotMove >= pivot)
                {
                    array[pivotMove] = array[pivotMove - 1];
                    pivotMove--;
                }

                // 把给定字符放到最左边
                array[pivot] = code;
                break;
            }

            pivot--;
        }
    }

    /// <summary>
    /// 后处理，用于修改文本的unicode编码和字符移位
    /// </summary>
    /// <param name="source">替换过编码映射的文本</param>
    /// <returns>处理之后的文本</returns>
    private static string PostProcess(string source)
    {
        byte[] bytes = Encoding.Unicode.GetBytes(source);
        StringBuilder sb = new();
        for (int j = 0; j < bytes.Length; j += 2)
        {
            sb.AppendFormat("u{0:X2}{1:X2}", bytes[j + 1], bytes[j]);
        }

        string fixStr = sb.ToString();
        Debug.LogError(fixStr);
        string[] codeArray = fixStr.Split("u");

        int index = 0;
        while (index < codeArray.Length)
        {
            string code = codeArray[index]; // 当前unicode码

            // 处理179A 重叠辅音位置错误
            if (code == "1026" && index > 0)
            {
                // 交换位置
                (codeArray[index - 1], codeArray[index]) = (code, codeArray[index - 1]);
            }

            // 处理 17C1 17C2 17C3 元音位置错误
            if ((code == "17C1" || code == "17C2" || code == "17C3") && index > 0)
            {
                MoveVowelToLeft(codeArray, index);
            }

            // 处理包围型元音
            if (code == "1060" || code == "1061" || code == "1062" || code == "1063" || code == "1064")
            {
                codeArray[index] = "17C1";
                MoveVowelToLeft(codeArray, index);
                index++; // 跳过下一个包围符
            }

            index++;
        }


        string finalText = "";

        foreach (string code in codeArray)
        {
            if (code != "")
                finalText += (char)int.Parse(code, NumberStyles.HexNumber);
        }

        return finalText;
    }

    /// <summary>
    /// 给定原柬埔寨语文本，返回修正后的文本
    /// </summary>
    /// <param name="source">原文本</param>
    /// <returns>修改后的文本</returns>
    public static string Adjust(string source)
    {
        source = Mapping52DoubleConsonants(source);
        source = Mapping5RoundVowel(source);
        source = PostProcess(source);
        return source;
    }
}