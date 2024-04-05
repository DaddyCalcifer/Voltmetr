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
        double mult_;
        public double Multiply {
            get { return mult_; }
            set
            {
                RotateToAnim(value_, value);
                this.mult_ = value;
            }
        }
        public void setArrowRotateTransform(RotateTransform arrow)
        {
            angle = arrow;
        }
        public double value {  
            get { return value_; }
            set {
                if (value_ > 150) value_ = 150;
                RotateToAnim(value,Multiply);
                this.value_ = value;
            } 
        }
        void RotateToAnim(double val, double multiply=1)
        {
            var new_val = val * multiply;
            if (new_val > 150)
            {
                new_val = 150;
                System.Windows.MessageBox.Show(
                    "При такой нагрузке реальное устройство выйдет из строя!", 
                    "Будьте внимательны!",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
            DoubleAnimation rotateAnim = new DoubleAnimation();
            double fix_ = 1 + new_val / 1000.0f;
            double fix_2 = 0;
            if(new_val > 35 && new_val < 101)
            {
                fix_2 += 1;
            }
            if (new_val > 100) { fix_ *= 0.9f; 
                fix_2 += 7;
                if (new_val > 100 && new_val < 140) fix_2 += 5;
                else fix_2 += 3;
            }
            rotateAnim.From = angle.Angle;
            rotateAnim.To = (((new_val+fix_2-(1))*fix_) - 80);
            rotateAnim.Duration = TimeSpan.FromSeconds(Math.Abs(angle.Angle-new_val)/120.0);

            angle.BeginAnimation(RotateTransform.AngleProperty, rotateAnim);
        }
    }
}
