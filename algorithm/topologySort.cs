using System;
using System.Collections.Generic;

namespace fastCSharp.algorithm
{
    /// <summary>
    /// 拓扑排序
    /// </summary>
    public static class topologySort
    {
        /// <summary>
        /// 拓扑排序器
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        private struct sorter<valueType>
        {
            /// <summary>
            /// 图
            /// </summary>
            private Dictionary<valueType, list<valueType>> graph;
            /// <summary>
            /// 排序结果
            /// </summary>
            private valueType[] values;
            /// <summary>
            /// 当前排序位置
            /// </summary>
            private int index;
            /// <summary>
            /// 是否反向排序
            /// </summary>
            private bool isDesc;
            /// <summary>
            /// 拓扑排序器
            /// </summary>
            /// <param name="graph">图</param>
            /// <param name="points">单点集合</param>
            /// <param name="isDesc">是否反向排序</param>
            public sorter(Dictionary<valueType, list<valueType>> graph, list<valueType> points, bool isDesc)
            {
                this.graph = graph;
                this.isDesc = isDesc;
                values = new valueType[graph.Count + points.count()];
                if (isDesc)
                {
                    index = points.count();
                    points.copyTo(values, 0);
                }
                else
                {
                    points.copyTo(values, index = graph.Count);
                }
            }
            /// <summary>
            /// 拓扑排序
            /// </summary>
            /// <returns>排序结果</returns>
            public valueType[] Sort()
            {
                list<valueType> points;
                if (isDesc)
                {
                    foreach (valueType point in graph.getArray(value => value.Key))
                    {
                        if (graph.TryGetValue(point, out points))
                        {
                            graph[point] = null;
                            foreach (valueType nextPoint in points) popDesc(nextPoint);
                            graph.Remove(point);
                            values[index++] = point;
                        }
                    }
                }
                else
                {
                    foreach (valueType point in graph.getArray(value => value.Key))
                    {
                        if (graph.TryGetValue(point, out points))
                        {
                            graph[point] = null;
                            foreach (valueType nextPoint in points) pop(nextPoint);
                            graph.Remove(point);
                            values[--index] = point;
                        }
                    }
                }
                return values;
            }
            /// <summary>
            /// 排序子节点
            /// </summary>
            /// <param name="point">子节点</param>
            private void pop(valueType point)
            {
                list<valueType> points;
                if (graph.TryGetValue(point, out points))
                {
                    if (points == null) log.Default.Throw("拓扑排序循环", false, false);
                    graph[point] = null;
                    foreach (valueType nextPoint in points) pop(nextPoint);
                    graph.Remove(point);
                    values[--index] = point;
                }
            }
            /// <summary>
            /// 排序子节点
            /// </summary>
            /// <param name="point">子节点</param>
            private void popDesc(valueType point)
            {
                list<valueType> points;
                if (graph.TryGetValue(point, out points))
                {
                    if (points == null) log.Default.Throw("拓扑排序循环", false, false);
                    graph[point] = null;
                    foreach (valueType nextPoint in points) popDesc(nextPoint);
                    graph.Remove(point);
                    values[index++] = point;
                }
            }
        }
        /// <summary>
        /// 拓扑排序
        /// </summary>
        /// <typeparam name="valueType">数据类型</typeparam>
        /// <param name="edges">边集合</param>
        /// <param name="points">无边点集合</param>
        /// <param name="isDesc">是否反向排序</param>
        /// <returns>排序结果</returns>
        public static valueType[] Sort<valueType>(ICollection<keyValue<valueType, valueType>> edges, HashSet<valueType> points
            , bool isDesc = false)
        {
            Dictionary<valueType, list<valueType>> graph = new Dictionary<valueType, list<valueType>>();
            list<valueType>.unsafer edgePoints = default(list<valueType>.unsafer);
            if (edges != null)
            {
                edgePoints = new list<valueType>(edges.Count).Unsafer;
                list<valueType> values;
                foreach (keyValue<valueType, valueType> edge in edges)
                {
                    if (!graph.TryGetValue(edge.Key, out values)) graph.Add(edge.Key, values = new list<valueType>());
                    values.Add(edge.Value);
                    edgePoints.Add(edge.Value);
                }
                valueType[] pointArray = edgePoints.Array;
                int count = 0;
                if (points.count() != 0)
                {
                    while (count != pointArray.Length)
                    {
                        valueType point = pointArray[count];
                        if (graph.ContainsKey(point) || points.Contains(point)) break;
                        ++count;
                    }
                    if (count != pointArray.Length)
                    {
                        for (int index = count; ++index != pointArray.Length; )
                        {
                            valueType point = pointArray[index];
                            if (!graph.ContainsKey(point) && !points.Contains(point)) pointArray[count++] = point;
                        }
                    }
                }
                else
                {
                    while (count != pointArray.Length && !graph.ContainsKey(pointArray[count])) ++count;
                    if (count != pointArray.Length)
                    {
                        for (int index = count; ++index != pointArray.Length; )
                        {
                            valueType point = pointArray[index];
                            if (!graph.ContainsKey(point)) pointArray[count++] = point;
                        }
                    }
                }
                edgePoints.AddLength(count - pointArray.Length);
            }
            list<valueType> pointList = edgePoints.List;
            if (points.count() != 0)
            {
                if (pointList == null) pointList = points.getList();
                else
                {
                    foreach (valueType point in points)
                    {
                        if (!graph.ContainsKey(point)) pointList.Add(point);
                    }
                }
            }
            return new sorter<valueType>(graph, pointList, isDesc).Sort();
        }
    }
}
