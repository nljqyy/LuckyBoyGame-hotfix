using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedConcroller {
    private static SpeedConcroller speedConcroller;
    private const float MAX_SPEED = 500;
    private const float MIN_SPEED = 300;
    private float speed = 260;
    private long lastTime;
    //加速度状态（递增或递减）
    private Astats astats;
    private Direction direction;
    //递减或递增参数
    private float mAParameter;
    //额定加速度
    private float a;
    //当前加速度
    private float currentA;
    //模式控制（即难度控制）
    private Model mModel;
    //时间区间
    private long time;
    private SpeedConcroller() { }
    public static SpeedConcroller getInstance() {
        if (speedConcroller == null) {
            speedConcroller = new SpeedConcroller();
        }
        return speedConcroller;
    }
    //加速度递增或递减
    private enum Astats {
        increase, degression
    }
    //速度增长 或 减小
    private enum Direction {
        up,down
    };
    //五种难度模式
    public enum Model
    {
        model1=1, model2, model3, model4, model5
    }
   public void setModel(Model m) {
        mModel = m;
        switch (m) {
            case Model.model1:
                speed = 300;
                break;
            case Model.model2:
                speed = 400;
                break;
            case Model.model3:
                time = 10000;
                a = 1000;
                astats = Astats.increase;
                break;
            case Model.model4:
                time = 6000;
                a = 2;
                astats = Astats.increase;
                break;
            case Model.model5:
                astats = Astats.increase;
                break;
        }
        if (a > 0)
        {
            direction = Direction.up;
        }
        else {
            direction= Direction.down;
        }
        //speed = 200;
        //mAParameter = a / 1000f;
    }
    // Update is called once per frame
    public float Update () {
        if (mModel == Model.model1|| mModel == Model.model2) {
            return speed;
                }

        if (Direction.up == direction) {
            //if (astats == Astats.increase)
            //{
            //    speed += a;
            //    //currentA = currentA + Time.deltaTime * mAParameter;
            //    //if (currentA >= a)
            //    //{
            //    //    astats = Astats.degression;
            //    //}
            //}
            speed += Time.deltaTime * a;
            //else {
            //currentA = currentA - Time.deltaTime * mAParameter;
            //if (currentA <= 0)
            //{
            //    direction= Direction.down;
            //}
            //}
            if (MAX_SPEED <= speed) {
                direction = Direction.down;
            }
        }
        else {
            speed -= Time.deltaTime * a;
            if (MIN_SPEED >= speed)
            {
                direction = Direction.up;
            }
            //if (astats == Astats.degression)
            //{
            //    currentA = currentA - Time.deltaTime * mAParameter;
            //    if (currentA <= -a)
            //    {
            //        astats = Astats.increase;
            //    }
            //}
            //else
            //{
            //    currentA = currentA + Time.deltaTime * mAParameter;
            //    if (currentA >= 0)
            //    {
            //        direction = Direction.up;
            //    }
            //}
        }
        speed =speed+currentA;
        return speed;
    }
}
