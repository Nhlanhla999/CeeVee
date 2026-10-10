using System.Collections.Generic;
using CeeVee.Models;

namespace CeeVee.Services
{
    public static class CVPageSizeService
    {
        public static List<CVPageSizeOption> GetPageSizes()
        {
            return new List<CVPageSizeOption>
            {
                new CVPageSizeOption(
                    CVPageSize.A4,
                    "A4",
                    793.7,
                    1122.5,
                    "210 × 297 mm"),

                new CVPageSizeOption(
                    CVPageSize.A3,
                    "A3",
                    1122.5,
                    1587.4,
                    "297 × 420 mm"),

                new CVPageSizeOption(
                    CVPageSize.A5,
                    "A5",
                    559.4,
                    793.7,
                    "148 × 210 mm"),

                new CVPageSizeOption(
                    CVPageSize.Letter,
                    "Letter",
                    816,
                    1056,
                    "8.5 × 11 in"),

                new CVPageSizeOption(
                    CVPageSize.Legal,
                    "Legal",
                    816,
                    1344,
                    "8.5 × 14 in"),

                new CVPageSizeOption(
                    CVPageSize.Executive,
                    "Executive",
                    696,
                    1008,
                    "7.25 × 10.5 in"),

                new CVPageSizeOption(
                    CVPageSize.B5,
                    "B5",
                    665.2,
                    943.9,
                    "176 × 250 mm"),

                new CVPageSizeOption(
                    CVPageSize.B4,
                    "B4",
                    944,
                    1336,
                    "250 × 353 mm"),

                new CVPageSizeOption(
                    CVPageSize.Tabloid,
                    "Tabloid",
                    1056,
                    1632,
                    "11 × 17 in"),

                new CVPageSizeOption(
                    CVPageSize.CustomResume,
                    "Custom Resume",
                    760,
                    1080,
                    "Custom CV format")
            };
        }
    }
}
