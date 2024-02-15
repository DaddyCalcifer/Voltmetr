using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Voltmetr.Logic
{
    public class ArrowManager
    {
        public ArrowManager(RotateTransform angle)
        {
            this.angle = angle;
        }
        RotateTransform angle;
        double value_ = -90;
        public double value {  
            get { return value_; }
            set {
                RotateToAnim(value);
                //rtaFix1(value);
                //rtaFix2(value);
                this.value_ = value;
            } 
        }
        void RotateToAnim(double new_val)
        {
            DoubleAnimation rotateAnim = new DoubleAnimation();
            rotateAnim.From = value;
            rotateAnim.To = new_val;
            rotateAnim.Duration = TimeSpan.FromSeconds(Math.Abs(value-new_val)/100.0);

            angle.BeginAnimation(RotateTransform.AngleProperty, rotateAnim);
        }
        void rtaFix1(double new_val)
        {
            DoubleAnimation rotateAnim_fix = new DoubleAnimation();
            rotateAnim_fix.From = new_val * 1.1f;
            rotateAnim_fix.To = new_val * 0.9f;
            rotateAnim_fix.Duration = TimeSpan.FromSeconds(0.12);

            angle.BeginAnimation(RotateTransform.AngleProperty, rotateAnim_fix);
        }
        void rtaFix2(double new_val)
        {
            DoubleAnimation rotateAnim_fix2 = new DoubleAnimation();
            rotateAnim_fix2.From = new_val * 0.9f;
            rotateAnim_fix2.To = new_val;
            rotateAnim_fix2.Duration = TimeSpan.FromSeconds(0.12);

            angle.BeginAnimation(RotateTransform.AngleProperty, rotateAnim_fix2);
        }
    }
}
