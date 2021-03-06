﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public class Graph
    {
        public class Vertex
        {
            public Point point;
            public List<Vertex> neighbours;
            public int degree;
            public Vertex(Point p)
            {
                point = p;
                neighbours = new List<Vertex>();
                degree = 0;
            }

            public Vertex(Point p, List<Vertex> a)
            {
                point = p;
                neighbours = a;
                degree = 0;
            }

            public override bool Equals(System.Object obj)
            {
                // If parameter is null return false.
                if (obj == null)
                {
                    return false;
                }

                // If parameter cannot be cast to Point return false.
                Vertex p = obj as Vertex;
                if ((System.Object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return this == p;
            }

            public bool Equals(Vertex p)
            {
                // If parameter is null return false:
                if ((object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return this == p;
            }

            public override int GetHashCode()
            {
                return point.GetHashCode();
            }

            public static bool operator ==(Vertex a, Vertex b)
            {
                if (ReferenceEquals(a, b))
                {
                    return true;
                }

                // If one is null, but not both, return false.
                if (((object)a == null) || ((object)b == null))
                {
                    return false;
                }

                // Return true if the fields match:
                return a.point == b.point;
            }

            public static bool operator ==(Vertex a, Point b)
            {
                if (ReferenceEquals(a, b))
                {
                    return true;
                }

                // If one is null, but not both, return false.
                if (((object)a == null) || ((object)b == null))
                {
                    return false;
                }

                // Return true if the fields match:
                return a.point == b;
            }

            public static bool operator !=(Vertex a, Vertex b)
            {
                return !(a == b);
            }

            public static bool operator !=(Vertex a, Point b)
            {
                return !(a == b);
            }
        }

        public class Edge
        {
            public Vertex vertex1;
            public Vertex vertex2;
            public Edge(Vertex n1, Vertex n2)
            {
                vertex1 = n1;
                vertex2 = n2;
            }

            public override bool Equals(System.Object obj)
            {
                // If parameter is null return false.
                if (obj == null)
                {
                    return false;
                }

                // If parameter cannot be cast to Point return false.
                Edge p = obj as Edge;
                if ((System.Object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return this == p;
            }

            public bool Equals(Edge p)
            {
                // If parameter is null return false:
                if ((object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return this == p;
            }

            public override int GetHashCode()
            {
                return vertex1.GetHashCode() ^ vertex2.GetHashCode();
            }

            public static bool operator ==(Edge a, Edge b)
            {
                if (ReferenceEquals(a, b))
                {
                    return true;
                }

                // If one is null, but not both, return false.
                if (((object)a == null) || ((object)b == null))
                {
                    return false;
                }

                // Return true if the fields match:
                return (a.vertex1 == b.vertex1 && a.vertex2 == b.vertex2) || (a.vertex1 == b.vertex2 && a.vertex2 == b.vertex1);
            }

            public static bool operator !=(Edge a, Edge b)
            {
                return !(a == b);
            }
        }

        List<Vertex> vertices;
        List<Edge> edges;
        List<int> foundCycle;

        public Graph()
        {
            vertices = new List<Vertex>();
            edges = new List<Edge>();
        }

        public IEnumerable<Vertex> GetVertices()
        {
            return vertices;
        }

        public IEnumerable<Edge> GetEdges()
        {
            return edges;
        }

        public void AddEdge(Point p1, Point p2)
        {
            AddEdge(new Vertex(p1), new Vertex(p2));
        }

        public void AddEdge(Edge edgeToAdd)
        {
            AddEdge(edgeToAdd.vertex1, edgeToAdd.vertex2);
        }

        public void AddEdge(Vertex vertex1, Vertex vertex2)
        {
            if (vertex1 == vertex2 || ContainsEdge(vertex1, vertex2))
                return;

            Vertex vertexToConnect1 = null;
            Vertex vertexToConnect2 = null;

            if (!vertices.Any(x => x == vertex1 || x == vertex2))
            {
                vertices.Add(vertex1);
                vertices.Add(vertex2);
                vertexToConnect1 = vertex1;
                vertexToConnect2 = vertex2;
            }
            else
            {
                Vertex foundVertex1 = vertices.Find(x => x == vertex1);
                Vertex foundVertex2 = vertices.Find(x => x == vertex2);
                if (foundVertex1 == null)
                {
                    vertices.Add(vertex1);
                }
                else if (foundVertex2 == null)
                {
                    vertices.Add(vertex2);
                }
                vertexToConnect1 = foundVertex1 ?? vertex1;
                vertexToConnect2 = foundVertex2 ?? vertex2;
            }

            edges.Add(new Edge(vertexToConnect1, vertexToConnect2));
            vertexToConnect1.neighbours.Add(vertexToConnect2);
            vertexToConnect2.neighbours.Add(vertexToConnect1);
        }

        public void RemoveEdge(Edge edgeToRemove)
        {
            Edge edge = edges.Find(x => x == edgeToRemove);

            if (edge == null)
                return;

            edge.vertex1.neighbours.Remove(edge.vertex2);
            edge.vertex2.neighbours.Remove(edge.vertex1);

            edges.Remove(edge);
        }

        public void RemoveEdge(Vertex vertex1, Vertex vertex2)
        {
            RemoveEdge(new Edge(vertex1, vertex2));
        }

        public void RemoveEdge(Point p1, Point p2)
        {
            RemoveEdge(new Edge(new Vertex(p1), new Vertex(p2)));
        }

        bool ContainsEdge(Vertex vertex1, Vertex vertex2)
        {
            Edge constructedEdge = new Edge(vertex1, vertex2);
            foreach (var edge in edges)
            {
                if (edge == constructedEdge)
                    return true;
            }
            return false;
        }

        bool ContainsEdge(Point p1, Point p2)
        {
            return ContainsEdge(new Vertex(p1), new Vertex(p2));
        }

        public void AddVertex(Point p)
        {
            bool searchP = false;
            for (int i = 0; i < vertices.Count; i++)
            {
                if (vertices[i].point == p)
                {
                    searchP = true;
                    break;
                }
            }
            if (!searchP)
                vertices.Add(new Vertex(p, new List<Vertex>()));
        }

        //public void maxPath(List<Point> path, double maxWeight)
        //{
        //    foreach(var node in nodes)
        //    {
        //        if (node.adj.Count == 1 && path.FindIndex(new Predicate<Point>(s => s == node.adj[0])) != -1)
        //            break;
        //        foreach(Point a in node.adj)
        //        {
        //            if(path.FindIndex(new Predicate<Point>(s=> s == a)) == -1)
        //            {
        //                path.Add(a);
        //                maxWeight += getWeight(node.point, a);
        //                maxPath(path, maxWeight);
        //            }

        //        }
        //    }
        //}

        //public void setWeight()
        //{
        //    double weight
        //    foreach (Point a in nodes[0].adj) // 
        //}

        public List<Point> GetPath()
        {
            List<Point> path = new List<Point>();
            //double maxWeight = 0;
            //int indexStart = 0;
            //for(int i = 0; i < nodes.Count; i++)
            //{
            //    if(nodes[i].weight > maxWeight)
            //    {
            //        maxWeight = nodes[i].weight;
            //        indexStart = i;
            //    }
            //}
            //path.Add(nodes[indexStart].point);
            
            int maxLevel = 0;
            int nowLevel = 1;
            int currentIndex = 0;
            while(nowLevel > maxLevel)
            {
                maxLevel = nowLevel;
                nowLevel = 1;
                for (int i = 0; i < vertices.Count; i++)
                {
                    vertices[i].degree = 0;
                }
                vertices[currentIndex].degree = 1;
                bool flag = true;
                while (flag)
                {
                    for (int i = 0; i < vertices.Count; i++)
                    {
                        if (vertices[i].degree == nowLevel)
                            for (int j = 0; j < vertices[i].neighbours.Count; j++)
                            {
                                if (vertices[i].neighbours[j].degree == 0)
                                {
                                    vertices[i].neighbours[j].degree = vertices[i].degree + 1;
                                    
                                }
                            }
                    }
                    nowLevel++;
                    flag = false;
                    for (int i = 0; i < vertices.Count; i++)
                    {
                        if (vertices[i].degree == 0)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                for (int i = 0; i < vertices.Count; i++)
                {
                    if (vertices[i].degree == nowLevel)
                    {
                        currentIndex = i;
                        break;
                    }
                }

            }
            path.Add(vertices[currentIndex].point);
            Vertex v = vertices[currentIndex];

            for (nowLevel--;nowLevel > 0; nowLevel--)
            {
                for(int j = v.neighbours.Count - 1; j >= 0; j--)
                {
                    if(v.neighbours[j].degree == nowLevel)
                    {
                        path.Add(v.neighbours[j].point);
                        v = vertices.Find(x => x.point == v.neighbours[j].point);
                        break;
                    }
                }
            }
            

            return path;
        }

        public List<List<Point>> GetAllPaths()
        {
            List<List<Point>> allPaths = new List<List<Point>>();
            //Graph copyCraph = this;
            for (int i = 0; i < vertices.Count; i++)
                vertices[i].degree = 0;
            int maxLevel = 0;
            int currentLevel = 0;
            while (currentLevel - maxLevel <= 1) 
            {
                List<Point> path = new List<Point>();
                currentLevel = maxLevel;
                maxLevel = 0;
                int nowLevel = 1;
                int currentIndex = 0;
                for (int i = 0; i < vertices.Count; i++)
                    if (vertices[i].degree != -1)
                        currentIndex = i;

                while (nowLevel > maxLevel)
                {
                    maxLevel = nowLevel;
                    nowLevel = 1;
                    for (int i = 0; i < vertices.Count; i++)
                    {
                        if (vertices[i].degree != -1)
                        {
                            vertices[i].degree = 0;
                        }
                    }
                    //if (vertices[currentIndex].degree != -1)
                    vertices[currentIndex].degree = 1;
                    bool flag = true;
                    while (flag)
                    {
                        for (int i = 0; i < vertices.Count; i++)
                        {
                            if (vertices[i].degree == nowLevel)
                                for (int j = 0; j < vertices[i].neighbours.Count; j++)
                                {
                                    if (vertices[i].neighbours[j].degree == 0)
                                    {
                                        vertices[i].neighbours[j].degree = vertices[i].degree + 1;

                                    }
                                }
                        }
                        nowLevel++;
                        flag = false;
                        for (int i = 0; i < vertices.Count; i++)
                        {
                            if (vertices[i].degree == 0)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    for (int i = 0; i < vertices.Count; i++)
                    {
                        if (vertices[i].degree == nowLevel)
                        {
                            currentIndex = i;
                            break;
                        }
                    }

                }
                path.Add(vertices[currentIndex].point);
                vertices[currentIndex].degree = -1;
                Vertex v = vertices[currentIndex];

                for (nowLevel--; nowLevel > 0; nowLevel--)
                {
                    for (int j = v.neighbours.Count - 1; j >= 0; j--)
                    {
                        if (v.neighbours[j].degree == nowLevel)
                        {
                            path.Add(v.neighbours[j].point);
                            if (nowLevel == 1)
                                v.neighbours[j].degree = -1;
                            v = vertices.Find(x => x.point == v.neighbours[j].point);
                            break;
                        }
                    }
                }
                //vertices[currentIndex].degree = -1;

                allPaths.Add(path);
            }
            //this.edges = copyCraph.edges;
            //this.vertices = copyCraph.vertices;
            //this.painted = copyCraph.painted;
            allPaths.RemoveAt(allPaths.Count - 1);
            return allPaths;
        }

        public double GetWeight(Point p1, Point p2)
        {
            return Math.Pow(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2), 0.5);
        }

        public void RemoveCycles(int maxCycle)
        {
            while (true)
            {
                SearchCycle(maxCycle);
                if (foundCycle.Count == 0)
                    return;

                double xNew = 0;
                double yNew = 0;
                for (int i = 0; i < foundCycle.Count; i++)
                {
                    int j = i + 1 == foundCycle.Count ? 0 : i + 1;
                    xNew += vertices[foundCycle[i]].point.X;
                    yNew += vertices[foundCycle[i]].point.Y;
                    RemoveEdge(vertices[foundCycle[i]], vertices[foundCycle[j]]);
                }
                xNew /= foundCycle.Count;
                yNew /= foundCycle.Count;
                Vertex newVertex = new Vertex(new Point(xNew, yNew));

                List<Vertex> unconnectedVertices = new List<Vertex>();
                foreach (var index in foundCycle)
                {
                    if (vertices[index].neighbours.Count != 0)
                        AddEdge(vertices[index], newVertex);
                    else
                        unconnectedVertices.Add(vertices[index]);
                }

                foreach (var v in unconnectedVertices)
                {
                    vertices.Remove(v);
                }
            }
        }

        bool[] painted;

        private void SearchCycle(int maxCycle)
        {
            foundCycle = new List<int>();
            painted = new bool[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                for (int k = 0; k < vertices.Count; k++)
                    painted[k] = false;
                List<int> cycle = new List<int>
                {
                    i
                };
                if (DFScycle(i, i, -1, cycle, maxCycle))
                    return;
            }
        }

        private bool DFScycle(int u, int endV, int unavailableVertex, List<int> cycle, int maxCountCycle)
        {
            if (cycle.Count == maxCountCycle)
                return false;

            if (u != endV)
                painted[u] = true;
            else if (cycle.Count > 2)
            {
                cycle.Reverse();
                foundCycle.AddRange(cycle.Take(cycle.Count - 1));
                return true;
            }
            foreach (var neighbour in vertices[u].neighbours)
            {
                int indexNeighbour = vertices.IndexOf(neighbour);
                if (indexNeighbour == unavailableVertex)
                    continue;

                if (!painted[indexNeighbour])
                {
                    List<int> cycleCopy = new List<int>(cycle)
                    {
                        indexNeighbour
                    };
                    if (DFScycle(indexNeighbour, endV, u, cycleCopy, maxCountCycle))
                        return true;
                    painted[indexNeighbour] = false;
                }
            }
            return false;
        }
    }
}
