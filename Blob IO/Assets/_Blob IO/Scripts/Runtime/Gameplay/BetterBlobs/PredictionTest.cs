using System;
using UnityEngine;

namespace BlobIO.BetterBlobs
{
    public class PredictionTest : MonoBehaviour
    {
        [SerializeField] double x = 0, y = 0.5;
        [SerializeField] double xn = 1;
        [SerializeField] double h = 0.2;

        static double f(double x, double y)
        {
            double v = y - 2 * x * x + 1;
            return v;
        }

        // predicts the next value for a given (x, y)
        // and step size h using Euler method
        static double predict(double x, double y, double h)
        {
            // value of next y(predicted) is returned
            double y1p = y + h * f(x, y);
            return y1p;
        }

        // corrects the predicted value
        // using Modified Euler method
        static double correct(double x, double y,
            double x1, double y1,
            double h)
        {
            // (x, y) are of previous step
            // and x1 is the increased x for next step
            // and y1 is predicted y for next step
            double e = 0.00001;
            double y1c = y1;

            do
            {
                y1 = y1c;
                y1c = y + 0.5 * h * (f(x, y) + f(x1, y1));
            } while (Math.Abs(y1c - y1) > e);

            // every iteration is correcting the value
            // of y using average slope
            return y1c;
        }

        static void printFinalValues(double x, double xn,
            double y, double h)
        {
            double lastX = x;
            double lastY = y;
            
            while (x < xn)
            {
                double x1 = x + h;
                double y1p = predict(x, y, h);
                double y1c = correct(x, y, x1, y1p, h);
                x = x1;
                y = y1c;
                
                Debug.DrawLine(new Vector3((float)x, (float)y, 0f), new Vector3((float)lastX, (float)lastY, 0f), Color.red, 3f);

                lastX = x;
                lastY = y;
            }
        }

        [ContextMenu(nameof(Evaluate))]
        private void Evaluate()
        {
            printFinalValues(x, xn, y, h);
        }
    }
}