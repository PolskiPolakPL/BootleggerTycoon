using System;
namespace PolskiPolakPL.Utils
{
    /// <summary>
    /// Timer class from tutorial extended by PolskiPolakPL. You can find original tutorial
    /// <seealso href="https://youtu.be/pRjTM3pzqDw">here</seealso>
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// Remaning time in seconds.
        /// </summary>
        public float RemaningSeconds { get; private set; }



        /// <summary>
        /// Time passed in seconds.
        /// </summary>
        public float SecondsPassed { get; private set; } = 0;



        private bool isLooping = true;

        /// <summary>
        /// Public getter of 'isLooping' boolean.
        /// </summary>
        public bool IsLooping
        {
            get { return isLooping; }
            set { isLooping = value; }
        }



        /// <summary>
        /// Timer Action event invoked at the end of counting time.
        /// </summary>
        public event Action OnTimerElapsed;



        /// <summary>
        /// Timer Action event invoked every <c>Tick()</c>.
        /// </summary>
        public event Action OnTimerTick;



        /// <summary>
        /// Timer Action event invoked whet it's duration is changed.
        /// </summary>
        public event Action OnDurationChanged;



        private float duration;



        /// <summary>
        /// Constructor for Timer class.
        /// </summary>
        /// <param name="duration">Duration of the timer in seconds</param>
        /// <param name="isLooping">Controlls if Timer is looping. Default = <c>true</c></param>
        public Timer(float duration, bool isLooping = true)
        {
            this.duration = duration;
            RemaningSeconds = duration;
            this.isLooping = isLooping;
        }


        /// <summary>
        /// Changes base duration of the Timer.
        /// </summary>
        /// <param name="newDuration">sets new duration</param>
        /// <param name="resetCurrentTime">reserts remaning time back to the beginning. Default = <c>true</c></param>
        public void ChangeDuration(float newDuration, bool resetCurrentTime = true)
        {
            duration = newDuration;
            if (resetCurrentTime)
                RemaningSeconds = duration;
            OnDurationChanged?.Invoke();
        }



        /// <summary>
        /// Changes base duration of the Timer. Doesn't invoke <c>OnDurationChanged</c> action.
        /// </summary>
        /// <param name="newDuration"></param>
        /// <param name="resetCurrentTime"></param>
        public void ChangeDurationWithoutNotify(float newDuration, bool resetCurrentTime = true)
        {
            duration = newDuration;
            if (resetCurrentTime)
                RemaningSeconds = duration;
        }



        /// <summary>
        /// Method used to move time one tick. Recommended use in <c>Update()</c> or <c>FixedUpdate()</c> methods.
        /// </summary>
        /// <param name="deltaTime">time difference between ticks</param>
        /// <param name="invokeEvent">Controlls if Timer invokes <c>OnTimerChanged</c> Action. Deafault = <c>false</c></param>
        public void Tick(float deltaTime, bool invokeEvent = false)
        {
            if(RemaningSeconds == 0)
            {
                if (isLooping)
                    RemaningSeconds = duration;
                return;
            }
            RemaningSeconds -= deltaTime;
            SecondsPassed += deltaTime;
            if(invokeEvent)
                OnTimerTick?.Invoke();
            CheckForTimerEnd();
        }

        private void CheckForTimerEnd()
        {
            if(RemaningSeconds > 0)
                return;
            RemaningSeconds = 0;
            OnTimerElapsed?.Invoke();
        }
    }
}
