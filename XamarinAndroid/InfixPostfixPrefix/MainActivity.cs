using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace InfixPostfixPrefix {
    [Activity(Label = "InfixPostfixPrefix",MainLauncher = true,Icon = "@drawable/icon")]
    public class MainActivity : Activity {

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            //////////////

            var btn_in = FindViewById<ToggleButton>(Resource.Id.infix);
            var btn_pos = FindViewById<ToggleButton>(Resource.Id.posfix);
            var btn_pre = FindViewById<ToggleButton>(Resource.Id.prefix);

            btn_in.Click += delegate {
                btn_pre.Checked = false;
                btn_pos.Checked = false;
            };
            btn_pos.Click += delegate {
                btn_pre.Checked = false;
                btn_in.Checked = false;
            };
            btn_pre.Click += delegate {
                btn_pos.Checked = false;
                btn_in.Checked = false;
            };
            var button = FindViewById<Button>(Resource.Id.MyButton);
            button.Click += delegate {
                var converter = new ConvertTo();
                var plaintext = FindViewById<EditText>(Resource.Id.plain).Text;
                if(btn_pre.Checked) {
                    converter.PREstr = plaintext;
                    converter.PreToIn();
                    converter.PreToPost();
                }
                else if(btn_pos.Checked) {
                    converter.POSTstr = plaintext;
                    converter.PostToIn();
                    converter.PostToPre();
                }
                else {
                    converter.INstr = plaintext;
                    converter.InToPost();
                    converter.InToPre();
                }
                var answer1=FindViewById<EditText>(Resource.Id.AnswerInfix)  ;
                var answer2=FindViewById<EditText>(Resource.Id.AnswerPrefix) ;
                var answer3=FindViewById<EditText>(Resource.Id.AnswerPostfix);
                var answer4=FindViewById<EditText>(Resource.Id.AnswerValue);


                answer1.SetText("Infix :"+converter.INstr,TextView.BufferType.Editable);
                answer2.SetText("Prefix:"+converter.PREstr,TextView.BufferType.Editable);
                answer3.SetText("Postfix:"+converter.POSTstr,TextView.BufferType.Editable);
                answer4.SetText("Value:" + converter.Evaluate(),TextView.BufferType.Editable);
            };
        }
    }
}

