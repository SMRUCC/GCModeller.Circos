Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Karyotype
Imports LANS.SystemsBiology.NCBI.Extensions.LocalBLAST.Application
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic.DocumentFormat.Csv

Module Module1

    Sub Main()
        Dim fas As New FastaFile("G:\5.14.circos\6.22\Af293.fna")
        Dim maps As BlastnMapping() = "G:\5.14.circos\6.22\maps.MergeMappings-Trim.Full.Perfect.identities=0.9.Csv".LoadCsv(Of BlastnMapping)
        Dim genome As KaryotypeChromosomes = KaryotypeChromosomes.FromBlastnMappings(maps, fas)
        Call genome.GenerateDocument(0).SaveTo("x:/test.txt")
    End Sub
End Module
