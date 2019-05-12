namespace NeuralTrainings
{
    // Класс нейрона
    class Neuron
    {
        public Neuron()
        {
            Signal = false;
        }

        // Флаг о проходе сигнала
        public bool Signal { get; set; }
    }
}
