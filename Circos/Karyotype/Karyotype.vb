﻿Imports Microsoft.VisualBasic.Serialization

Namespace Karyotype

    ''' <summary>
    ''' The ideogram using karyotype file to define the genome skeleton information, which defines the name, size and color of chromosomes. 
    ''' </summary>
    ''' <remarks>
    ''' A simple karyotype with 5 chromosomes:
    '''
    ''' ```
    ''' chr1 5Mb
    ''' chr2 10Mb
    ''' chr3 20Mb
    ''' chr4 50Mb
    ''' chr5 100Mb
    ''' ```
    ''' 
    ''' The format Of this file Is
    '''
    ''' ```
    ''' chr - CHRNAME CHRLABEL START End COLOR
    ''' ```
    ''' 
    ''' In data files, chromosomes are referred To by CHRNAME. 
    ''' On the image, they are labeled by CHRLABEL
    '''
    ''' Colors are taken from the spectral Brewer palette. 
    ''' To learn about Brewer palettes, see (www.colorbrewer.org)[http://www.colorbrewer.org]
    ''' </remarks>
    Public Class Karyotype : Implements IKaryotype

        Public Property chrName As String Implements IKaryotype.chrName
        Public Property chrLabel As String
        Public Property start As Integer Implements IKaryotype.start
        Public Property [end] As Integer Implements IKaryotype.end
        Public Property color As String Implements IKaryotype.color

        Public Overrides Function ToString() As String Implements IKaryotype.GetData
            Return $"chr - {chrName} {chrLabel} {start} {[end]} {color}"
        End Function
    End Class

    Public Interface IKaryotype
        Property start As Integer
        Property [end] As Integer
        Property color As String
        Property chrName As String

        Function GetData() As String
    End Interface

    ''' <summary>
    ''' Bands are defined using
    '''
    ''' ```
    ''' band CHRNAME BANDNAME BANDLABEL START End COLOR
    ''' ```
    ''' 
    ''' Currently ``BANDNAME`` And ``BANDLABEL`` are Not used
    '''
    ''' Colors correspond To levels Of grey To match
    ''' conventional shades Of banding found In genome
    ''' browsers. For example, ``gpos25`` Is a light grey.
    '''
    ''' For examples Of real karyotype files, see
    ''' ``data/karyotype`` In the Circos distribution directory.
    ''' Or data/karyotype In the course directory.
    ''' </summary>
    Public Class Band : Implements IKaryotype

        Public Property chrName As String Implements IKaryotype.chrName
        Public Property color As String Implements IKaryotype.color
        Public Property [end] As Integer Implements IKaryotype.end
        Public Property start As Integer Implements IKaryotype.start
        Public Property bandX As String
        Public Property bandY As String

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function

        Public Function GetData() As String Implements IKaryotype.GetData
            Return $"band {chrName} {bandX} {bandY} {start} {[end]} {color}"
        End Function
    End Class
End Namespace