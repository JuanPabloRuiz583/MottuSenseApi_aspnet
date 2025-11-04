namespace Sprint.ml;
using Microsoft.ML.Data;

public class MotoInput
{
    [LoadColumn(0)]
    public float PatioId { get; set; }

    [LoadColumn(1)]
    public float ClienteId { get; set; }

    [LoadColumn(2)]
    public float NumeroChassiLength { get; set; }

    [LoadColumn(3)]
    public string PredictedStatus { get; set; }
}

public class MotoPrediction
{
    // Status previsto da moto (ex.: DISPONIVEL, EM_USO, etc.)
    [ColumnName("PredictedLabel")]
    public string PredictedStatus { get; set; }
}