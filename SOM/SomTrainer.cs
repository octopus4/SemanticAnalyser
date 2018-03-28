using System;
using System.Threading;

namespace SOM
{
    /// <summary>
    /// Trainer of the <see cref="KohonenMap"/> 
    /// </summary>
    public class SomTrainer
    {
        /// <summary>
        /// Instance of the <see cref="SomTrainer"/>
        /// </summary>
        private static SomTrainer Instance { get; set; }
        /// <summary>
        /// Thread that learns <see cref="KohonenMap"/> through epochs
        /// </summary>
        private Thread LearningThread { get; set; }

        /// <summary>
        /// Instance of the <see cref="KohonenMap"/> that should be learned
        /// </summary>
        public KohonenMap Som { get; private set; }
        /// <summary>
        /// Number of the current epoch
        /// </summary>
        public int Epoch { get; private set; }
        /// <summary>
        /// Current error
        /// </summary>
        public double Error { get; private set; }
        /// <summary>
        /// Shows if the learning process has been stopped
        /// </summary>
        public bool IsLearningStopped { get; private set; }

        /// <summary>
        /// Event of the <see cref="LearningThread"/> tick
        /// </summary>
        public event EventHandler ThreadTick;
        /// <summary>
        /// Event of the <see cref="LearningThread"/> end
        /// </summary>
        public event EventHandler ThreadDone;

        /// <summary>
        /// Initializes a new instance of the <see cref="SomTrainer"/>
        /// </summary>
        private SomTrainer()
        {
            IsLearningStopped = true;
        }

        /// <summary>
        /// Returns an existing instance of the <see cref="SomTrainer"/> or creates a new one
        /// </summary>
        /// <returns></returns>
        public static SomTrainer GetInstance()
        {
            if (Instance == null)
            {
                Instance = new SomTrainer();
            }
            return Instance;
        }

        /// <summary>
        /// Starts the learning thread
        /// </summary>
        /// <param name="som"></param>
        public void Start(KohonenMap som)
        {
            Som = som;
            Epoch = 0;
            Error = 0;
            if (IsLearningStopped)
            {
                LearningThread = new Thread(LearnSom);
                LearningThread.Start();
            }
            IsLearningStopped = false;
        }

        /// <summary>
        /// Stops the learning thread
        /// </summary>
        /// <returns></returns>
        public KohonenMap Stop()
        {
            IsLearningStopped = true;
            return Som;
        }

        /// <summary>
        /// Learns an instance of the <see cref="KohonenMap"/> while map is not learned or learning process is not stopped
        /// </summary>
        private void LearnSom()
        {
            while (!(IsLearningStopped || Som.IsLearned))
            {
                Error = Som.LearnEpoch();
                ThreadTick?.Invoke(this, EventArgs.Empty);
                Epoch++;
            }
            if (!IsLearningStopped)
            {
                IsLearningStopped = true;
            }
            Som.EndLearning();
            ThreadDone?.Invoke(this, EventArgs.Empty);
        }
    }
}
