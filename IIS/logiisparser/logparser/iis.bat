%2\LogParser -e:10 -i:IISW3C "SELECT sc-status as Status, COUNT(*) as Requests INTO %2\%3 FROM   %5\*%1  GROUP BY sc-status order by sc-status" -o:TPL -tpl:%2\iis.tpl 
rem pause
%2\LogParser -e:10 -i:IISW3C "SELECT cs-uri-stem as url, DIV(SUM(time-taken),1000) as Seconds, Count(time-taken) as Requests, DIV(Seconds ,Requests) as TimeExecuting   INTO %2\%4 FROM   %5\*%1  GROUP BY cs-uri-stem Having SUM(time-taken)>0 and Seconds>0 order by Seconds   desc" -o:TPL -tpl:%2\iistime.tpl
rem pause