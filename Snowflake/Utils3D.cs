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
        public static SceneNode DrawLine(SceneManager mSceneMgr, Vector3 start, Vector3 end) {
            ManualObject line = mSceneMgr.CreateManualObject("line" + numLines);
            SceneNode lineNode = mSceneMgr.RootSceneNode.CreateChildSceneNode("line" + numLines + "_node");

            MaterialPtr lineMaterial = MaterialManager.Singleton.Create("line" + numLines + "_material", "Default");
            lineMaterial.ReceiveShadows = false;
            lineMaterial.SetLightingEnabled(true);
            lineMaterial.SetDiffuse(0, 0, 1, 0);
            lineMaterial.SetAmbient(0, 0, 1);
            lineMaterial.SetSelfIllumination(0, 0, 1);

            line.Begin("line" + numLines + "_material", RenderOperation.OperationTypes.OT_LINE_LIST);
            line.Position(start);
            line.Position(end);
            line.End();

            lineNode.AttachObject(line);
            ++numLines;
            return lineNode;
        }
    }
}
