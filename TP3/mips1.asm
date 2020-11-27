	.text
main:
lui $20, 0x1001
ori $8, $20, 0
ori $9, $20, 4
lw $8, 0($8)
lw $9, 0($9)
addu $10, $8, $9
ori $20, $20, 8
sw $10, 0($20) #guardar 5 na memoria
lw $13, 0($20) #ler 5
j jump
teste:slt $14, $8, $9
slt $14, $9, $8
and $15, $8, $9
sll $16, $15, 2
jump:beq $0, $16, teste

    .data
a:  .word 2
b:  .word 3
c:  .word 0
