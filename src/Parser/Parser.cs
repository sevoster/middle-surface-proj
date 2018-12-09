using System;
using System.Collections.Generic;
using System.Linq;
using MidSurfaceNameSpace.Primitive;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Windows;

namespace MidSurfaceNameSpace.IO
{
    public class FigureParser : IFigureParser
    {
        public IFigure ParseFile(string filePath)
        {
            Shape2D deserializedShape = Parse(filePath);
            
            if (deserializedShape == null)
            {
                return null;
            }
            //Temporary check here
            foreach (var contour in deserializedShape.Contour)
            {
                if (contour.JointsOfSegments.Count() != contour.Segments.Count())
                {
                    return null;
                }
            }

            return ConvertShapeToFigure(deserializedShape);
        }

        public void ExportFile(IMidSurface f,  string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MidSurfaceNameSpace.Component.MSModel));
            FileStream fs = new FileStream(filePath, FileMode.Open);
            serializer.Serialize(fs, f);
            fs.Close();
        }

        private Shape2D Parse(string filePath)
        {
            if (!ValidateXMLBySchema(filePath))
            {
                return null;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Shape2D));
            FileStream fs = new FileStream(filePath, FileMode.Open);
            Shape2D shape = (Shape2D)serializer.Deserialize(fs);
            fs.Close();

            return shape;
        }

        private bool ValidateXMLBySchema(string filePath)
        {
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var resourceName = "Parser.ShapeSchema.xsd";

                Stream stream = assembly.GetManifestResourceStream(resourceName);
                XmlReaderSettings settings = new XmlReaderSettings();
                
                settings.Schemas.Add("", XmlReader.Create(stream));
                settings.ValidationType = ValidationType.Schema;

                XmlReader reader = XmlReader.Create(filePath, settings);
                XmlDocument document = new XmlDocument();
                document.Load(reader);

                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);

                // the following call to Validate succeeds.
                document.Validate(eventHandler);
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Console.WriteLine("Error: {0}", e.Message);
                    break;
                case XmlSeverityType.Warning:
                    Console.WriteLine("Warning {0}", e.Message);
                    break;
            }
        }

        private IFigure ConvertShapeToFigure(Shape2D shape)
        {
            Figure result = new Figure();
            foreach (var contour in shape.Contour)
            {
                Primitive.Contour convertContour = new Primitive.Contour();
                List<ParserPoint> points = new List<ParserPoint>();

                for (int i = 0; i < contour.Segments.Count() - 1; i++)
                {
                    points.Add(contour.JointsOfSegments[i]);

                    foreach (var point in contour.Segments[i])
                    {
                        points.Add(point);
                    }

                    points.Add(contour.JointsOfSegments[i + 1]);

                    convertContour.Add(new Segment(new BezierCurve(), ConvertPoints(points)));
                    points.Clear();
                }

                points.Add(contour.JointsOfSegments.Last());

                foreach (var point in contour.Segments.Last())
                {
                    points.Add(point);
                }

                points.Add(contour.JointsOfSegments.First());

                convertContour.Add(new Segment(new BezierCurve(), ConvertPoints(points)));
                result.Add(convertContour);
            }
            return result;
        }

        private List<Point> ConvertPoints(List<ParserPoint> points)
        {
            List<Point> convertedPoints = new List<Point>();
            foreach (var point in points)
            {
                convertedPoints.Add(new Point(point.X, point.Y));
            }
            return convertedPoints;
        }
    }
}
