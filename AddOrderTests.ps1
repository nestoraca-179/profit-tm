param(
    [int]$OnlyCase = 0,
    [string]$OverrideConnId = "",
    [string]$OverrideBranchId = ""
)

$ErrorActionPreference = "Continue"
$endpoint = "http://localhost:54034/api/v1/orders"
$docPrefix = "9" + (Get-Date -Format "MMddHHmmss")

function New-Order {
    param([string]$DocNum)

    return [ordered]@{
        doc_num = $DocNum
        descrip = ""
        co_cli = "J412960121"
        co_tran = "NA"
        co_mone = "US$"
        co_ven = "001"
        co_cond = "001"
        fec_emis = "2025-03-28T00:00:00"
        fec_venc = "2025-03-28T00:00:00"
        fec_reg = "2025-03-28T00:00:00"
        anulado = $false
        status = "2"
        n_control = "0"
        ven_ter = $false
        tasa = 69.44
        porc_desc_glob = $null
        monto_desc_glob = 0.0
        porc_reca = $null
        monto_reca = 0.0
        total_bruto = 64579.2
        monto_imp = 10332.67
        monto_imp2 = 0.0
        monto_imp3 = 0.0
        otros1 = 0.0
        otros2 = 0.0
        otros3 = 0.0
        total_neto = 74911.87
        saldo = 74911.87
        dir_ent = ""
        comentario = "0"
        dis_cen = $null
        feccom = $null
        numcom = $null
        contrib = $true
        impresa = $false
        seriales_s = $null
        salestax = $null
        impfis = $null
        impfisfac = $null
        campo1 = "J407167561 AGENTE ADUANAL FRONTERA VENCOL, C.A"
        campo2 = "24/3/2025 4:25:00 p. m."
        campo3 = "GGZ2411705"
        campo4 = "COCTG"
        campo5 = "VEPBL"
        campo6 = "SHOWCASE FREEZER,ISLAND FREEZER"
        campo7 = "CMA CGM ALEXANDRIA"
        campo8 = "0NGFCN1MA"
        co_us_in = "NACA"
        co_sucu_in = "002"
        fe_us_in = "2025-03-28T13:42:00.143"
        co_us_mo = "PROFIT"
        co_sucu_mo = $null
        fe_us_mo = "2025-03-28T13:42:00.143"
        revisado = $null
        trasnfe = $null
        validador = $null
        rowguid = "AFD86726-D8CF-4C0F-8DE1-DC3996F4DF05"
        co_cta_ingr_egr = $null
        saPedidoVentaReng = @(
            [ordered]@{
                reng_num = 1; doc_num = $DocNum; co_art = "410104003-001"; des_art = $null; co_alma = "002"
                total_art = 3.0; stotal_art = 0.0; co_uni = "UND"; sco_uni = $null; co_precio = "001"
                prec_vta = 20832.0; prec_vta_om = $null; porc_desc = $null; monto_desc = 0.0
                tipo_imp = "1"; tipo_imp2 = $null; tipo_imp3 = $null; porc_imp = 16.0; porc_imp2 = 0.0; porc_imp3 = 0.0
                monto_imp = 0.0; monto_imp2 = 0.0; monto_imp3 = 0.0; reng_neto = 62496.0; pendiente = 0.0; pendiente2 = 0.0
                lote_asignado = $false; tipo_doc = $null; num_doc = $null; rowguid_doc = $null
                monto_desc_glob = 0.0; monto_reca_glob = 0.0; otros1_glob = 0.0; otros2_glob = 0.0; otros3_glob = 0.0
                monto_imp_afec_glob = 0.0; monto_imp2_afec_glob = 0.0; monto_imp3_afec_glob = 0.0
                total_dev = 0.0; monto_dev = 0.0; otros = 0.0; comentario = ""; dis_cen = $null
                co_us_in = "NACA"; co_sucu_in = "002"; fe_us_in = "2025-03-28T13:42:00.157"
                co_us_mo = "PROFIT"; co_sucu_mo = $null; fe_us_mo = "2025-03-28T13:42:00.157"
                revisado = $null; trasnfe = $null; rowguid = "32B0E9CA-CBFB-4818-B9A1-383C5248395F"
            },
            [ordered]@{
                reng_num = 2; doc_num = $DocNum; co_art = "410106003-001"; des_art = $null; co_alma = "002"
                total_art = 3.0; stotal_art = 0.0; co_uni = "UND"; sco_uni = $null; co_precio = "001"
                prec_vta = 694.4; prec_vta_om = $null; porc_desc = $null; monto_desc = 0.0
                tipo_imp = "1"; tipo_imp2 = $null; tipo_imp3 = $null; porc_imp = 16.0; porc_imp2 = 0.0; porc_imp3 = 0.0
                monto_imp = 0.0; monto_imp2 = 0.0; monto_imp3 = 0.0; reng_neto = 2083.2; pendiente = 0.0; pendiente2 = 0.0
                lote_asignado = $false; tipo_doc = $null; num_doc = $null; rowguid_doc = $null
                monto_desc_glob = 0.0; monto_reca_glob = 0.0; otros1_glob = 0.0; otros2_glob = 0.0; otros3_glob = 0.0
                monto_imp_afec_glob = 0.0; monto_imp2_afec_glob = 0.0; monto_imp3_afec_glob = 0.0
                total_dev = 0.0; monto_dev = 0.0; otros = 0.0; comentario = ""; dis_cen = $null
                co_us_in = "NACA"; co_sucu_in = "002"; fe_us_in = "2025-03-28T13:42:00.160"
                co_us_mo = "PROFIT"; co_sucu_mo = $null; fe_us_mo = "2025-03-28T13:42:00.160"
                revisado = $null; trasnfe = $null; rowguid = "123F67F7-16EF-45F8-A39C-B9441C7365AB"
            }
        )
    }
}

function New-Case {
    param([int]$Number, [string]$Name, [string]$Expected, [object]$Body)
    [pscustomobject]@{ Number = $Number; Name = $Name; Expected = $Expected; Body = $Body }
}

$cases = @()
$cases += New-Case 1 "Exito ConnId 14 BranchId 002" "201" ([ordered]@{ ConnId = "14"; BranchId = "002"; UserId = "NACA"; Order = New-Order ($docPrefix + "01") })
$cases += New-Case 2 "Exito ConnId 14 sin BranchId" "201 o error de BD por sucursal nula" ([ordered]@{ ConnId = "14"; BranchId = $null; UserId = "NACA"; Order = New-Order ($docPrefix + "02") })
$cases += New-Case 3 "Exito ConnId 15 BranchId 007" "201 o error de datos no existentes en BD 15" ([ordered]@{ ConnId = "15"; BranchId = "007"; UserId = "JPRZ"; Order = New-Order ($docPrefix + "03") })
$cases += New-Case 4 "Request nulo" "400" $null
$cases += New-Case 5 "ConnId faltante" "400" ([ordered]@{ BranchId = "002"; UserId = "NACA"; Order = New-Order ($docPrefix + "05") })
$cases += New-Case 6 "ConnId invalido" "400" ([ordered]@{ ConnId = "99"; BranchId = "002"; UserId = "NACA"; Order = New-Order ($docPrefix + "06") })
$cases += New-Case 7 "BranchId invalido para ConnId 14" "400" ([ordered]@{ ConnId = "14"; BranchId = "008"; UserId = "NACA"; Order = New-Order ($docPrefix + "07") })
$cases += New-Case 8 "BranchId 006 con ConnId 15" "400" ([ordered]@{ ConnId = "15"; BranchId = "006"; UserId = "NACA"; Order = New-Order ($docPrefix + "08") })
$cases += New-Case 9 "Order nulo" "400" ([ordered]@{ ConnId = "14"; BranchId = "002"; UserId = "NACA"; Order = $null })
$order10 = New-Order ($docPrefix + "10"); $order10.doc_num = ""
$cases += New-Case 10 "Doc_num faltante" "400" ([ordered]@{ ConnId = "14"; BranchId = "002"; UserId = "NACA"; Order = $order10 })
$order11 = New-Order ($docPrefix + "11"); $order11.saPedidoVentaReng = @()
$cases += New-Case 11 "Pedido sin renglones" "400" ([ordered]@{ ConnId = "14"; BranchId = "002"; UserId = "NACA"; Order = $order11 })
$order12 = New-Order ($docPrefix + "12"); $order12.saPedidoVentaReng = @($null)
$cases += New-Case 12 "Renglon nulo" "400" ([ordered]@{ ConnId = "14"; BranchId = "002"; UserId = "NACA"; Order = $order12 })
$order13 = New-Order ($docPrefix + "13"); $order13.saPedidoVentaReng[1].reng_num = 1
$cases += New-Case 13 "Renglones repetidos" "400" ([ordered]@{ ConnId = "14"; BranchId = "002"; UserId = "NACA"; Order = $order13 })
$order14 = New-Order ($docPrefix + "14"); $order14.saPedidoVentaReng[0].reng_num = 0
$cases += New-Case 14 "Renglon con numero invalido" "400" ([ordered]@{ ConnId = "14"; BranchId = "002"; UserId = "NACA"; Order = $order14 })
$order15 = New-Order ($docPrefix + "15"); $order15.saPedidoVentaReng[0].co_art = ""
$cases += New-Case 15 "Renglon sin articulo" "400" ([ordered]@{ ConnId = "14"; BranchId = "002"; UserId = "NACA"; Order = $order15 })

if ($OnlyCase -gt 0) {
    $cases = @($cases | Where-Object { $_.Number -eq $OnlyCase })
}

if ($OverrideConnId -or $OverrideBranchId) {
    foreach ($test in $cases) {
        if ($OverrideConnId) { $test.Body.ConnId = $OverrideConnId }
        if ($OverrideBranchId) { $test.Body.BranchId = $OverrideBranchId }
    }
}

Write-Host "Endpoint: $endpoint"
Write-Host "Casos: $($cases.Count)"
Write-Host "Prefijo de documentos: $docPrefix"

foreach ($test in $cases) {
    $json = if ($null -eq $test.Body) { "null" } else { $test.Body | ConvertTo-Json -Depth 20 -Compress }
    $tempFile = Join-Path $env:TEMP ("add-order-{0}.json" -f $test.Number)
    [System.IO.File]::WriteAllText($tempFile, $json, [System.Text.UTF8Encoding]::new($false))

    $curlOutput = & curl.exe -sS -X POST $endpoint -H "Content-Type: application/json" --data-binary "@$tempFile" -w "`nHTTP_STATUS:%{http_code}" 2>&1 | Out-String
    Remove-Item $tempFile -Force -ErrorAction SilentlyContinue

    Write-Host ""
    Write-Host ("[{0}] {1}" -f $test.Number, $test.Name)
    Write-Host ("Expected: {0}" -f $test.Expected)
    Write-Host $curlOutput.Trim()
}
