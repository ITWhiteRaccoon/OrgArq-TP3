main:
ori $8, $0, 2
ori $9, $0, 3
addu $10, $8, $9
lui $11, 129 #0b1000 0001
lui $12, 0x1001 #0x1001 0000 (primeiro endereco de memoria
sw $10, 0($12) #guardar 5 na memoria
lw $13, 0($12) #ler 5
j jump
teste:slt $14, $8, $9
slt $14, $9, $8
and $15, $8, $9
sll $16, $15, 2
jump:beq $0, $16, teste

.data
.asciiz "Testando"