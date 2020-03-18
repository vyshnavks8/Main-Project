using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANN
{
    public int numInputs;
    public int numOutputs;
    public int numHiddenLayer;
    public int numNeuronsInHiddenLayer;
    public double alpha;
    List<Layer> layers = new List<Layer>();
    public ANN(int nI,int nO,int nHL,int nNIHL,double a)
    {
        numInputs = nI;
        numOutputs = nO;
        numHiddenLayer = nHL;
        numNeuronsInHiddenLayer = nNIHL;
        alpha = a;
        if (numHiddenLayer > 0)
        {
            layers.Add(new Layer(numNeuronsInHiddenLayer, numInputs));
            for (int i = 0; i < numHiddenLayer - 1; i++)
            {
                layers.Add(new Layer(numNeuronsInHiddenLayer, numNeuronsInHiddenLayer));
            }
            layers.Add(new Layer(numOutputs, numNeuronsInHiddenLayer));
        }
        else
        {
            layers.Add(new Layer(numOutputs, numInputs));
        }   
    }
    public List<double> Train(List<double> inputValues, List<double> desiredOutputs)
    {
        List<double> output = new List<double>();
        output = CalcOutput(inputValues, desiredOutputs);
        UpdateWeights(output, desiredOutputs);
        return output;
    }
    public List<double> CalcOutput(List<double> inputValues, List<double> desiredOutputs)
    {
        List<double> inputs = new List<double>();
        List<double> outputs = new List<double>();
        int currentInput = 0;
        if (inputValues.Count != numInputs)
        {
            Debug.Log("ERROR:num of Input must be" + numInputs);
            return outputs;
        }
        inputs = new List<double>(inputValues);
        for (int i = 0; i < numHiddenLayer + 1; i++)// Loop through each Layer
        {
            if (i > 0)
            {
                inputs = new List<double>(outputs);
            }
            outputs.Clear();
            for (int j = 0; j < layers[i].numNeurons; j++)// Loop through each Neuron
            {
                double N = 0;
                layers[i].neurons[j].inputs.Clear();
                for (int k = 0; k < layers[i].neurons[j].numInputs; k++)// Loop through each input of Neuron
                {
                    layers[i].neurons[j].inputs.Add(inputs[k]);
                    N += layers[i].neurons[j].weights[k] * inputs[k];
                    currentInput++;
                }
                N -= layers[i].neurons[j].bias;
                if(i==numHiddenLayer)
                {
                    layers[i].neurons[j].output = ActivateFunction2(N);
                }
                else
                {
                    layers[i].neurons[j].output = ActivateFunction1(N);
                }
                
                outputs.Add(layers[i].neurons[j].output);
                currentInput = 0;
            }
        }
        return outputs; 
    }
    public string PrintWeight()
    {
        string weightString = "";
        foreach (Layer l in layers)
        {
            foreach (Neuron n in l.neurons)
            {
                foreach (double w in n.weights)
                {
                    weightString += w + ",";
                }
            }
        }
        return weightString;
    }
    public void LoadWeight(string weightString)
    {
        if (weightString == "")
            return;
        string[] weightValue = weightString.Split(',');
        int w = 0;
        foreach (Layer l in layers)
        {
            foreach (Neuron n in l.neurons)
            {
                for (int i = 0; i < n.weights.Count; i++)
                {
                    n.weights[i] = System.Convert.ToDouble(weightValue[w]);
                    w++;
                }
            }
        }

    }


    private void UpdateWeights(List<double> outputs, List<double> desiredOutputs)
    {
        double error;
        for (int i = numHiddenLayer; i >= 0  + 1; i--)//Backpropogation
        {
            for (int j = 0; j < layers[i].numNeurons; j++)
            {
                if (i == numHiddenLayer)
                {
                    error = desiredOutputs[j] - outputs[j];
                    layers[i].neurons[j].errorGradient = outputs[j] * (1 - outputs[j]) * error;//Delta rule
                }
                else
                {
                    layers[i].neurons[j].errorGradient = layers[i].neurons[j].output * (1 - layers[i].neurons[j].output);
                    double errorGradientSum=0;
                    for (int p = 0; p < layers[i+1].numNeurons; p++)
                    {
                        errorGradientSum += layers[i + 1].neurons[p].errorGradient * layers[i + 1].neurons[p].weights[j];
                    }
                    layers[i].neurons[j].errorGradient *= errorGradientSum;
                }
                for (int k = 0; k < layers[i].neurons[j].numInputs; k++)
                {
                    if (i == numHiddenLayer)
                    {
                        error = desiredOutputs[j] - outputs[j];
                        layers[i].neurons[j].weights[k] += alpha * layers[i].neurons[j].inputs[k] * error;
                    }
                    else 
                    {
                        layers[i].neurons[j].weights[k] += alpha * layers[i].neurons[j].inputs[k] * layers[i].neurons[j].errorGradient;
                    }
                    layers[i].neurons[j].bias += alpha * -1 * layers[i].neurons[j].errorGradient;
                }
            }
        }
    }
    private double ActivateFunction1(double n)
    {
        return Sigmoid(n);
    }
    private double ActivateFunction2(double n)
    {
        return Sigmoid(n);
    }

    private double Sigmoid(double n)
    {
        double k = (double)System.Math.Exp(n);
        return k / (1.0f + k);
    }
    private double TanH(double n)
    {
        double k = (double)System.Math.Exp(-2*n);
        return 2 / (1.0f + k)-1;
    }
}
