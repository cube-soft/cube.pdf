/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
namespace Cube.Pdf.Pages.App
{
    /* --------------------------------------------------------------------- */
    ///
    /// Aggregator
    ///
    /// <summary>
    /// CubePDF Page で発生するイベントを集約するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Aggregator : IAggregator
    {
        #region Events

        /* ----------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// ファイルを一覧に追加するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent<string[]> Add { get; } = new RelayEvent<string[]>();

        /* ----------------------------------------------------------------- */
        ///
        /// Preview
        ///
        /// <summary>
        /// ファイルのプレビューを行うイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent Preview { get; } = new RelayEvent();

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// ファイルを一覧から削除するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent Remove { get; } = new RelayEvent();

        /* ----------------------------------------------------------------- */
        ///
        /// Clear
        ///
        /// <summary>
        /// 全てのファイルを一覧から削除するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent Clear { get; } = new RelayEvent();

        /* ----------------------------------------------------------------- */
        ///
        /// Move
        ///
        /// <summary>
        /// ページを移動するイベントです。
        /// </summary>
        ///
        /// <remarks>
        /// Move イベントの Value に設定される値は移動量になります。
        /// </remarks>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent<int> Move { get; } = new RelayEvent<int>();

        /* ----------------------------------------------------------------- */
        ///
        /// Merge
        ///
        /// <summary>
        /// ファイルを結合するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent Merge { get; } = new RelayEvent();

        /* ----------------------------------------------------------------- */
        ///
        /// Split
        ///
        /// <summary>
        /// ファイルを分割するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent Split { get; } = new RelayEvent();

        /* ----------------------------------------------------------------- */
        ///
        /// Refresh
        ///
        /// <summary>
        /// 画面を再描画するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent Refresh { get; } = new RelayEvent();

        /* ----------------------------------------------------------------- */
        ///
        /// Version
        ///
        /// <summary>
        /// バージョン情報を表示するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent Version { get; } = new RelayEvent();

        #endregion
    }

    /* --------------------------------------------------------------------- */
    ///
    /// AggregatorExtension
    ///
    /// <summary>
    /// Aggregator の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class AggregatorExtension
    {
        /* ----------------------------------------------------------------- */
        ///
        /// GetEvents
        ///
        /// <summary>
        /// Aggregator オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">イベント集約オブジェクト</param>
        ///
        /// <returns>Aggregator オブジェクト</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static Aggregator GetEvents(this IAggregator src) => src as Aggregator;
    }
}
