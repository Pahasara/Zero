// Copyright (c) 2023 Dewnith Fernando @github.com/Pahasara.
// Licensed under the MIT license.

using System;

namespace Zero.Core
{
    public class Compute
    {
        public int getScrollBarLocation(int currentRow, int maxRows, int maxLength, int LENGTH_FIX)
        {
            maxLength -= LENGTH_FIX;
            double rowPercentage = currentRow / Convert.ToDouble(maxRows - 1);
            int x = ((int)(rowPercentage * maxLength) + LENGTH_FIX);
            return x;
        }

        public int getPercentage(int length, double maxLength)
        {
            double percentage = (length / Convert.ToDouble(maxLength));
            percentage *= 100;
            return (int)percentage;
        }

        public int getScrollBarLength(int maxRows)
        {
            double maxLength = 202;
            int minLength = 20;
            double length = maxLength / maxRows;
            if (length < minLength) 
                length = minLength;
            return (int) length;
        }
    }
}
