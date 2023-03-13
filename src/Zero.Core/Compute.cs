using System;

namespace Zero.Core
{
    public class Compute
    {
        public int getScroll_X(int currentRow, int maxRows, int progressBarMaxWidth, int LENGTH_FIX)
        {
            progressBarMaxWidth -= LENGTH_FIX;
            Double rowPercentage = currentRow / Convert.ToDouble(maxRows - 1);
            int x = ((int)(rowPercentage * progressBarMaxWidth) + LENGTH_FIX);
            return x;
        }

        public int getProgressValue(int progressBarValue, double progressBarMaxLength)
        {
            double percentage = (progressBarValue / Convert.ToDouble(progressBarMaxLength));
            percentage *= 100;
            return (int)percentage;
        }
    }
}
