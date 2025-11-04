namespace Sprint.ml
{
    using Microsoft.ML;

    public class MotoModelTrainer
    {
        public void TrainAndSaveModel(string dataPath, string modelPath)
        {
            var mlContext = new MLContext();

            // Carregar os dados
            var data = mlContext.Data.LoadFromTextFile<MotoInput>(
                dataPath, hasHeader: true, separatorChar: ',');

            // Definir o pipeline de treinamento
            var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", "PredictedStatus")
                .Append(mlContext.Transforms.Concatenate("Features", "PatioId", "ClienteId", "NumeroChassiLength"))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy())
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            // Treinar o modelo
            var model = pipeline.Fit(data);

            // Garante que o diretório existe antes de salvar o modelo
            var directory = Path.GetDirectoryName(modelPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Salvar o modelo treinado
            mlContext.Model.Save(model, data.Schema, modelPath);

            Console.WriteLine($"Modelo treinado e salvo em: {modelPath}");
        }

    }
}