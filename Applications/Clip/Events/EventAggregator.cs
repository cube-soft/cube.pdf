/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Affero General Public License as published
/// by the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Affero General Public License for more details.
///
/// You should have received a copy of the GNU Affero General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///
/* ------------------------------------------------------------------------- */
namespace Cube.Pdf.App.Clip
{
    /* --------------------------------------------------------------------- */
    ///
    /// EventAggregator
    /// 
    /// <summary>
    /// CubePDF Clip で発生するイベントを集約するクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public class EventAggregator : IEventAggregator
    {
        /* ----------------------------------------------------------------- */
        ///
        /// Open
        ///
        /// <summary>
        /// 添付元の PDF ファイルを開くイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent<string[]> Open { get; } = new RelayEvent<string[]>();

        /* ----------------------------------------------------------------- */
        ///
        /// Attach
        ///
        /// <summary>
        /// 添付ファイルを追加するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent<string[]> Attach { get; } = new RelayEvent<string[]>();

        /* ----------------------------------------------------------------- */
        ///
        /// Detach
        ///
        /// <summary>
        /// 添付ファイルを削除するイベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent Detach { get; } = new RelayEvent();

        /* ----------------------------------------------------------------- */
        ///
        /// Reset
        ///
        /// <summary>
        /// 添付ファイルを Open イベント発生直後の状態にリセットする
        /// イベントです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public RelayEvent Reset { get; } = new RelayEvent();
    }

    /* --------------------------------------------------------------------- */
    ///
    /// EventOperations
    /// 
    /// <summary>
    /// イベント関連の拡張メソッドを定義するクラスです。
    /// </summary>
    /// 
    /* --------------------------------------------------------------------- */
    public static class EventOperations
    {
        /* ----------------------------------------------------------------- */
        ///
        /// GetEvents
        ///
        /// <summary>
        /// イベントの一覧を取得します。
        /// </summary>
        /// 
        /// <param name="ea">IEventAggregator オブジェクト</param>
        /// 
        /// <returns>イベント一覧</returns>
        ///
        /* ----------------------------------------------------------------- */
        public static EventAggregator GetEvents(this IEventAggregator ea)
            => ea as EventAggregator;
    }
}
