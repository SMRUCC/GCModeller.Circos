Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Configurations.Nodes.Plots
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Karyotype
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.TrackDatas.NtProps
Imports LANS.SystemsBiology.NCBI.Extensions.LocalBLAST.Application
Imports LANS.SystemsBiology.SequenceModel.FASTA
Imports Microsoft.VisualBasic.DocumentFormat.Csv

Module Module1

    Sub Main()
        Try
            Call run()
        Catch ex As Exception
            Call ex.PrintException
        End Try

        Pause()
    End Sub

    Private Sub run()
        Dim fas As New FastaFile("G:\5.14.circos\6.22\Af293.fna")
        Dim maps As BlastnMapping() = "G:\5.14.circos\6.22\maps.MergeMappings-Trim.Full.Perfect.identities=0.9.Csv".LoadCsv(Of BlastnMapping)
        Dim genome As KaryotypeChromosomes = KaryotypeChromosomes.FromBlastnMappings(maps, fas)
        Call genome.GenerateDocument(0).SaveTo("x:/test.txt")

        Dim circos = CircosAPI.CreateDoc
        circos.SkeletonKaryotype = genome
        circos.Includes.Add(New Configurations.Ideogram(circos))
        circos.Includes.Add(New Configurations.Ticks(circos))

        circos.GetIdeogram.Ideogram.show_label = yes

        circos.AddPlotElement(New Histogram(New GCSkew(genome:=fas, karyotype:=circos.SkeletonKaryotype, SlideWindowSize:=4096, Steps:=1024, Circular:=False)))
        circos.AddPlotElement(New Histogram(New GeneGCContent(genome:=fas, karyotype:=circos.SkeletonKaryotype, winSize:=4096, steps:=1024, getValue:=Function(x) x.GC_AT)))

        Call circos.Save("x:\test/")
    End Sub
End Module
