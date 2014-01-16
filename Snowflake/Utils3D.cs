using System;
using System.Text;

using Mogre;
using Math = Mogre.Math;

namespace Snowflake {
    public class Utils3D {

        /// <summary>
        /// Rotates the specified vector clockwise by the specified angle, in radians
        /// </summary>
        /// <param name="vec">The vector2 to rotate</param>
        /// <param name="radians">The number of radians to rotate clockwise by</param>
        /// <returns>The rotated vector</returns>
        public static Vector2 RotateVector2(Vector2 vec, float rad) {
            return new Vector2(vec.x * Math.Cos(rad) + vec.y * Math.Sin(rad), vec.x * Math.Sin(rad) - vec.y * Math.Cos(rad));
        }

        private static int numLines = 0;
        public static ColourValue LineColour = ColourValue.Blue;
        public static SceneNode DrawLine(SceneManager mSceneMgr, Vector3 start, Vector3 end) {
            ManualObject line = mSceneMgr.CreateManualObject("line" + numLines);
            SceneNode lineNode = mSceneMgr.RootSceneNode.CreateChildSceneNode("line" + numLines + "_node");

            MaterialPtr lineMaterial = MaterialManager.Singleton.Create("line" + numLines + "_material", "Default");
            lineMaterial.ReceiveShadows = false;
            lineMaterial.SetLightingEnabled(true);
            lineMaterial.SetDiffuse(LineColour.r, LineColour.g, LineColour.b, 0);
            lineMaterial.SetAmbient(LineColour.r, LineColour.g, LineColour.b);
            lineMaterial.SetSelfIllumination(LineColour.r, LineColour.g, LineColour.b);

            line.Begin("line" + numLines + "_material", RenderOperation.OperationTypes.OT_LINE_LIST);
            line.Position(start);
            line.Position(end);
            line.End();

            lineNode.AttachObject(line);
            ++numLines;
            return lineNode;
        }
        public static SceneNode DrawRay(SceneManager mSceneMgr, Ray r) {
            return DrawLine(mSceneMgr, r.Origin, r.Origin + r.Direction * 99999);
        }
    }
}
