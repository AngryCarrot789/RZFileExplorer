using System;

namespace RZFileExplorer.Utils {
    public static class Maths {
        public static int Min(int a, int b) {
            return a <= b ? a : b;
        }

        public static int Min(int a, int b, int c) {
            int min = Min(a, b);
            return min <= c ? min : c;
        }

        public static int Min(int a, int b, int c, int d) {
            int min = Min(a, b, c);
            return min <= d ? min : d;
        }

        public static int Min(int a, int b, int c, int d, int e) {
            int min = Min(a, b, c, d);
            return min <= e ? min : e;
        }

        public static int Max(int a, int b) {
            return a >= b ? a : b;
        }

        public static int Max(int a, int b, int c) {
            int max = Max(a, b);
            return max >= c ? max : c;
        }

        public static int Max(int a, int b, int c, int d) {
            int max = Max(a, b, c);
            return max >= d ? max : d;
        }

        public static int Max(int a, int b, int c, int d, int e) {
            int max = Max(a, b, c, d);
            return max >= e ? max : e;
        }

        public static int Min(params int[] values) {
            if (values == null || values.Length == 0) {
                throw new ArgumentException("Array is null or empty");
            }

            int min = values[0];
            for (int i = 1; i < values.Length; ++i) {
                min = Math.Min(min, values[i]);
            }

            return min;
        }

        public static int Max(params int[] values) {
            if (values == null || values.Length == 0) {
                throw new ArgumentException("Array is null or empty");
            }

            int max = values[0];
            for (int i = 1; i < values.Length; ++i) {
                max = Math.Max(max, values[i]);
            }

            return max;
        }
    }
}