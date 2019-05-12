(deffunction ask-question (?question $?allowed-values)
   (print ?question)
   (bind ?answer (read))
   (if (lexemep ?answer) 
       then (bind ?answer (lowcase ?answer)))
   (while (not (member$ ?answer ?allowed-values)) do
      (print ?question)
      (bind ?answer (read))
      (if (lexemep ?answer) 
          then (bind ?answer (lowcase ?answer))))
   ?answer)

(deffunction yes-or-no-p (?question)
   (bind ?response (ask-question ?question yes no y n))
   (if (or (eq ?response yes) (eq ?response y))
       then yes 
       else no))

(defrule determine1 ""
   =>
   (assert (more200 (yes-or-no-p ">= 200 pts? (yes/no)?"))))

(defrule determine2 ""
   (more200 no)
   =>
   (assert (conc "sad ((((((((((((((((.")))

(defrule determine4 ""
   (more200 yes)      
   =>
   (assert (question2 (yes-or-no-p "Do you study at main house? (yes/no)?"))))

(defrule determine5 ""
   (more200 yes)   
   (question2 yes)
   =>
   (assert (conc "you will live on 9th may street!")))

(defrule determine6 ""
   (more200 yes)   
   (question2 no)
   =>
   (assert (conc "you will live at the complexx!")))

(defrule system-banner ""
  =>
  (println crlf "HELLO!" crlf))

(defrule print-repair ""
  (conc ?item)
  =>
  (println crlf "CONCLUSION: " crlf)
  (println " " ?item crlf))
