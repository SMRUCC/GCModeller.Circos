Imports System.Text.RegularExpressions
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Documents.Karyotype.Highlights
Imports LANS.SystemsBiology.Assembly.DOOR
Imports LANS.SystemsBiology.NCBI.Extensions.LocalBLAST.Application.RpsBLAST
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.DocumentFormat.Csv
Imports Microsoft.VisualBasic.Linq.Extensions

Partial Module CLI

    <ExportAPI("/DOOR.COGs", Usage:="/DOOR.COGs /DOOR <genome.opr> [/out <out.COGs.Csv>]")>
    Public Function DOOR_COGs(args As CommandLine.CommandLine) As Integer
        Dim inFile As String = args("/DOOR")
        Dim out As String = args.GetValue("/out", inFile.TrimFileExt & ".COGs.Csv")
        Dim DOOR As DOOR = DOOR_API.Load(inFile)
        Dim COGs As MyvaCOG() = DOOR.Genes.ToArray(
            Function(x) New MyvaCOG With {
                .COG = x.COG_number,
                .Description = x.Product,
                .Length = x.Length,
                .Category = Regex.Split(x.COG_number, "\d+").Last,
                .MyvaCOG = x.COG_number,
                .QueryName = x.Synonym}).OrderBy(Function(x) x.QueryName).ToArray
        Return COGs.SaveTo(out).CLICode
    End Function

    <ExportAPI("/Regulons.Dumps", Usage:="/Regulons.Dumps /in <genomes.bbh.DIR> /ptt <genome.ptt> [/out <out.Csv>]")>
    Public Function DumpNames(args As CommandLine.CommandLine) As Integer
        Dim inDIR As String = args("/in")
        Dim ptt As String = args("/ptt")
        Dim out As String = args.GetValue("/out", inDIR & ".Names.Csv")
        Dim gbPTT = LANS.SystemsBiology.Assembly.NCBI.GenBank.TabularFormat.PTT.Load(ptt)
        Dim names = NameExtensions.DumpNames(inDIR, gbPTT)
        Return names.SaveTo(out).CLICode
    End Function
End Module
