namespace Cube.Pdf.Ghostscript
{
    /* --------------------------------------------------------------------- */
    ///
    /// Orientation
    ///
    /// <summary>
    /// ページの向きを定義した列挙型です。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public enum Orientation
    {
        /// <summary>自動</summary>
        Auto = 10,
        /// <summary>縦向き</summary>
        Portrait = 0,
        /// <summary>縦向き（180 度回転）</summary>
        PortraitReverse = 2,
        /// <summary>横向き</summary>
        Landscape = 3,
        /// <summary>横向き（180 度回転）</summary>
        LandscapeReverse = 1,
    }
}
