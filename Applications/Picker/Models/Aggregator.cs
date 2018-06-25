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
namespace Cube.Pdf.App.Picker
{
    /* --------------------------------------------------------------------- */
    ///
    /// Aggregator
    ///
    /// <summary>
    /// CubePDF ImagePicker で発生するイベントを集約するクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class Aggregator : IAggregator
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Save
        ///
        /// <summary>
        /// 画像を保存するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent<int[]> Save { get; } = new RelayEvent<int[]>();

        /* ----------------------------------------------------------------- */
        ///
        /// SaveComplete
        ///
        /// <summary>
        /// 画像の保存が完了した事を表すイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent SaveComplete { get; } = new RelayEvent();

        /* ----------------------------------------------------------------- */
        ///
        /// Preview
        ///
        /// <summary>
        /// プレビュー画面を表示するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent Preview { get; } = new RelayEvent();

        /* ----------------------------------------------------------------- */
        ///
        /// PreviewImage
        ///
        /// <summary>
        /// 画像のプレビューを表示するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent PreviewImage { get; } = new RelayEvent();

        /* ----------------------------------------------------------------- */
        ///
        /// Remove
        ///
        /// <summary>
        /// 画像を一覧から削除するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent Remove { get; } = new RelayEvent();

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
        /// Aggregator で定義されているイベント群にアクセス可能な
        /// オブジェクトを取得します。
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
