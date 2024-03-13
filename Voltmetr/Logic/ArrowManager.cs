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
        double value_;
        public double Multyply = 1f;
        public double value {  
            get { return value_; }
            set {
                if (value_ * Multyply > Multyply*150) value_ = Multyply*150;
                RotateToAnim(value);
                this.value_ = value;
            } 
        }
        void RotateToAnim(double new_val)
        {
            if(new_val*Multyply > Multyply*150) new_val = Multyply*150;
            DoubleAnimation rotateAnim = new DoubleAnimation();
            double fix_ = 1 + new_val / 1000.0f;
            double fix_2 = 0;
            if(new_val * Multyply > 35 * Multyply && new_val * Multyply < 101 * Multyply)
            {
                fix_2 += 1;
            }
            if (new_val * Multyply > 100 * Multyply) { fix_ *= 0.9f; 
                fix_2 += 7;
                if (new_val * Multyply > 100 * Multyply && new_val * Multyply < 140* Multyply) fix_2 += 5;
                else fix_2 += 3;
            }
            rotateAnim.From = (((value + fix_2 - (1)) * fix_) * Multyply - 80);
            rotateAnim.To = (((new_val+fix_2-(1))*fix_) * Multyply - 80);
            rotateAnim.Duration = TimeSpan.FromSeconds(Math.Abs(value-new_val)/120.0);

            angle.BeginAnimation(RotateTransform.AngleProperty, rotateAnim);
        }
    }
}
