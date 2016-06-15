Imports System.Text.RegularExpressions
Imports System.Text
Imports LANS.SystemsBiology.Assembly.KEGG.DBGET
Imports LANS.SystemsBiology.DatabaseServices
Imports LANS.SystemsBiology.InteractionModel
Imports LANS.SystemsBiology.InteractionModel.Regulon

Namespace Documents.Karyotype

    Public Class PhenotypeRegulation : Inherits GenomeDescription

        ''' <summary>
        ''' Family, Regulators
        ''' </summary>
        ''' <remarks></remarks>
        Dim RegulatorFamilies As KeyValuePair(Of String, String())()
        ''' <summary>
        ''' Phenotype Class, GeneList
        ''' </summary>
        ''' <remarks></remarks>
        Dim PhenoTypeAssociations As KeyValuePair(Of String, String())()
        ''' <summary>
        ''' Regulator, Genes
        ''' </summary>
        ''' <remarks></remarks>
        Dim Regulations As KeyValuePair(Of String, String())()

        Sub New(Regulations As IEnumerable(Of IRegulon), Pathways As IEnumerable(Of bGetObject.Pathway))
            Dim PathwayGenes = (From Pathway In Pathways
                                Select PathwayId = Pathway.EntryId,
                                    PathwayGenesId = Pathway.GetPathwayGenes).ToArray
            RegulatorFamilies = Regprecise.FamilyStatics2(Regulations)

            Dim PathwayFunctions As Dictionary(Of String, BriteHEntry.Pathway) =
                BriteHEntry.Pathway.LoadDictionary

            Dim LQuery = (From Pathway As bGetObject.Pathway
                          In Pathways.AsParallel
                          Where Not Pathway.Genes.IsNullOrEmpty
                          Let PathwayId As String = Pathway.EntryId
                          Let [Class] As BriteHEntry.Pathway = PathwayFunctions(Regex.Match(PathwayId, "\d{5}").Value)
                          Select Phenotype = [Class].Category,
                              AssociationGenes = Pathway.GetPathwayGenes).ToArray
            PhenoTypeAssociations = (From Phenotype As String
                                     In (From item In LQuery Select item.Phenotype Distinct).ToArray
                                     Let AssociatedGene As String() = (From item In LQuery
                                                                       Where String.Equals(Phenotype, item.Phenotype)
                                                                       Select item.AssociationGenes).MatrixToVector
                                     Select New KeyValuePair(Of String, String())(Phenotype, AssociatedGene)).ToArray
            Me.Regulations = (From Regulator As String
                              In (From item In Regulations Select item.TFlocusId Distinct).ToArray
                              Let RegulatedGene = (From item In Regulations Where String.Equals(item.TFlocusId, Regulator) Select item.RegulatedGenes).MatrixToVector
                              Select New KeyValuePair(Of String, String())(Regulator, (From strId As String
                                                                                       In RegulatedGene
                                                                                       Select strId Distinct).ToArray)).ToArray
        End Sub

        Protected Overrides Function GenerateDocument() As String
            Dim sBuilder As StringBuilder = New StringBuilder(1024)
            Dim i As Integer = 0
            For Each PhenoType In PhenoTypeAssociations
                i += 1
                Call sBuilder.AppendLine(String.Format("chr - ch{0} {0} 0 {1} chr{0}", i, PhenoType.Value.Count))
            Next

            Return sBuilder.ToString
        End Function

        Public Overrides ReadOnly Property Max As Double
            Get
                Return 1
            End Get
        End Property

        Public Overrides ReadOnly Property Min As Double
            Get
                Return 0
            End Get
        End Property

        Public Overrides ReadOnly Property AutoLayout As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property Size As Integer
            Get
                Return -1
            End Get
        End Property
    End Class
End Namespace