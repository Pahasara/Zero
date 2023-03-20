using System;
using System.Xml.Schema;

namespace Zero.Core
{
    public class Compute
    {
        public int getScrollBarLocation(int currentRow, int maxRows, int progressBarMaxWidth, int LENGTH_FIX)
        {
            progressBarMaxWidth -= LENGTH_FIX;
            double rowPercentage = currentRow / Convert.ToDouble(maxRows - 1);
            int x = ((int)(rowPercentage * progressBarMaxWidth) + LENGTH_FIX);
            return x;
        }

        public int getPercentage(int progressBarValue, double progressBarMaxLength)
        {
            double percentage = (progressBarValue / Convert.ToDouble(progressBarMaxLength));
            percentage *= 100;
            return (int)percentage;
        }

        public int getScrollBarLength(int mRows)
        {
            double maxLength = 202;
            int minLength = 20;
            double length = maxLength / mRows;
            if (length < minLength) 
                length = minLength;
            return (int) length;
        }
    }
}
