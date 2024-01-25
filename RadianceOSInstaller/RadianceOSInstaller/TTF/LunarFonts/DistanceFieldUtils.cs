using System;
using System.Linq;

namespace LunarLabs.Fonts {
    public static class DistanceFieldUtils {
        /*
        Computes a distance field transform of a high resolution binary source channel and returns the result as a low resolution channel.

        scale_down : The amount the source channel will be scaled down.
        A value of 8 means the destination image will be 1/8th the size of the source

        spread: The spread in pixels before the distance field clamps to (zero/one). 
        The valueis specified in units of the destination image. The spread in the source image will be spread*scale_down.
        */
        public static GlyphBitmap CreateDistanceField(GlyphBitmap source, int scale, float spread) {
            var result = new GlyphBitmap(source.Width / scale, source.Height / scale);

            var values = source.Pixels.Select(x => (x / 255.0f) - 0.5f).ToArray();

            for (int y = 0; y < result.Height; y++) {
                for (int x = 0; x < result.Width; x++) {
                    var sd = SignedDistance(values, source.Width, source.Height, x * scale, y * scale, spread);
                    var n = (sd + spread) / (spread * 2.0f);

                    var c = (byte)(n * 255);
                    var offset = x + y * result.Width;
                    result.Pixels[offset] = c;
                }
            }

            return result;
        }

        private static float SignedDistance(float[] source, int w, int h, int cx, int cy, float clamp) {
            var cd = source[cx + cy * w];

            int min_x = cx - (int)(Math.Floor(clamp) - 1);
            if (min_x < 0) {
                min_x = 0;
            }

            int max_x = cx + (int)(Math.Floor(clamp) + 1);
            if (max_x >= w) {
                max_x = w - 1;
            }

            float distance = clamp;
            for (int dy = 0; dy < (int)(Math.Floor(clamp) + 1); dy++) {
                if (dy > distance) {
                    continue;
                }

                if (cy - dy >= 0) {
                    int y1 = cy - dy;
                    for (int x = min_x; x <= max_x; x++) {
                        if (x - cx > distance) {
                            continue;
                        }

                        float d = source[x + y1 * w];
                        if (cd * d < 0) {
                            float d2 = (y1 - cy) * (y1 - cy) + (x - cx) * (x - cx);
                            if (d2 < (distance * distance)) { distance = (float)Math.Sqrt(d2); }
                        }
                    }
                }

                if (dy != 0 && cy + dy < h) {
                    int y2 = cy + dy;

                    for (int x = min_x; x < max_x; x++) {
                        if (x - cx > distance) {
                            continue;
                        }

                        float d = source[x + y2 * w];
                        if (cd * d < 0) {
                            float d2 = (y2 - cy) * (y2 - cy) + (x - cx) * (x - cx);
                            if (d2 < distance * distance) {
                                distance = (float)Math.Sqrt(d2);
                            }
                        }
                    }
                }


            }

            if (cd > 0)
                return distance;
            else
                return -distance;
        }
    }
}