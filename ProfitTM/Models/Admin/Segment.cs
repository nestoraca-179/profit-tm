using System;
using System.Linq;
using System.Collections.Generic;

namespace ProfitTM.Models
{
    public class Segment : ProfitAdmManager
    {
        public saSegmento GetSegmentByID(string id)
        {
            saSegmento segment;

            try
            {
                segment = db.saSegmento.AsNoTracking().SingleOrDefault(s => s.co_seg == id);
            }
            catch (Exception ex)
            {
                segment = null;
                Incident.CreateIncident("ERROR BUSCANDO SEGMENTO " + id, ex);
            }

            return segment;
        }

        public List<saSegmento> GetAllSegments()
        {
            List<saSegmento> segments;

            try
            {
                segments = db.saSegmento.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                segments = null;
                Incident.CreateIncident("ERROR BUSCANDO SEGMENTOS", ex);
            }

            return segments;
        }
    }
}